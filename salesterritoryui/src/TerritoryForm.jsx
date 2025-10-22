// Form component for creating and editing territories
import { useState, useEffect } from 'react';

export function TerritoryForm({ territory, onSave, onCancel }) {
    const [formData, setFormData] = useState({
        name: '',
        zipCodes: '',
        demographics: '{}',
    });
    const [errors, setErrors] = useState({}); // Field-specific validation errors from the API
    const [serverError, setServerError] = useState(''); // Generic server errors

    useEffect(() => {
        if (territory) {
            setFormData({
                name: territory.name,
                zipCodes: territory.zipCodes.join(', '),
                // Format demographics as pretty JSON for editing
                demographics: JSON.stringify(territory.demographics, null, 2),
            });
        } else {
            // Reset form for new territory
            setFormData({ name: '', zipCodes: '', demographics: '' });
        }
    }, [territory]);
    
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };
    
    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrors({}); // Clear previous errors
        setServerError('');
        
        // Validate JSON format before submission
        let demographicsObj;
        try {
            demographicsObj = JSON.parse(formData.demographics);
        } catch (err) {
            setErrors({'demographics': 'Demographics must be valid JSON e.g. ' + demographicsPlaceholder});
            return;
        }

        const submissionData = {
            ...territory, // Preserve ID for updates
            name: formData.name,
            zipCodes: formData.zipCodes.split(',').map(zip => zip.trim()).filter(zip => zip),
            demographics: demographicsObj,
        };

        try {
            await onSave(submissionData);
        } catch (error) {
            if (typeof error === 'object' && error !== null) {
                // API validation errors (400 response)
                setErrors(error);
            } else {
                // Server errors (500, network failures)
                setServerError(error.toString());
            }
        }
    };

    const demographicsPlaceholder = `{
    "households": 15000,
    "median_income": 85000,
    "primary_language": "English"
}`;

    return (
        <div className="modal-backdrop">
            <div className="modal-content">
                <h2>{territory ? 'Edit Territory' : 'Add New Territory'}</h2>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="name">Territory Name</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            value={formData.name}
                            onChange={handleChange}
                            placeholder="e.g., Northern Utah County"
                            required
                        />
                        {errors.name && <p className="error-message">{errors.name}</p>}
                    </div>
                    <div className="form-group">
                        <label htmlFor="zipCodes">Zip Codes (comma-separated)</label>
                        <input
                            type="text"
                            id="zipCodes"
                            name="zipCodes"
                            value={formData.zipCodes}
                            onChange={handleChange}
                            placeholder="e.g., 84005, 84043, 84045"
                            required
                        />
                        {errors.zipcodes && <p className="error-message">{errors.zipcodes}</p>}
                    </div>
                    <div className="form-group">
                        <label htmlFor="demographics">Demographics (JSON format)</label>
                        <textarea
                            id="demographics"
                            name="demographics"
                            value={formData.demographics}
                            onChange={handleChange}
                            placeholder={demographicsPlaceholder}
                            rows="5"
                        />
                    </div>
                    {errors.demographics && <p className="error-message">{errors.demographics}</p>}
                    {/* Display the generic server error here */}
                    {serverError && (
                        <div className="server-error-message">
                        <p>{serverError}</p>
                        </div>
                    )}
                    <div className="form-actions">
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" className="btn" onClick={onCancel}>Cancel</button>
                    </div>
                </form>
            </div>
        </div>
    );
}