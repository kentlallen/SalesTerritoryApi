// src/TerritoryForm.jsx

import { useState, useEffect } from 'react';

export function TerritoryForm({ territory, onSave, onCancel }) {
    const [formData, setFormData] = useState({
        name: '',
        zipCodes: '',
        demographics: '{}',
    });
    const [errors, setErrors] = useState({}); // State to hold validation errors, mapping field names to error messages

    useEffect(() => {
        if (territory) {
            setFormData({
                name: territory.name,
                zipCodes: territory.zipCodes.join(', '),
                // Convert the demographics object to a nicely formatted JSON string for the textarea
                demographics: JSON.stringify(territory.demographics, null, 2),
            });
        } else {
            // Reset for new territory
            setFormData({ name: '', zipCodes: '', demographics: '' });
        }
    }, [territory]);
    
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };
    
    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrors({}); // Clear previous errors on a new submission
        
        if(territory) {
            if(formData.name.length === 0 || formData.zipCodes.length === 0 || formData.demographics.length === 0) {
                setFormData({ name: territory.name, zipCodes: territory.zipCodes, demographics: JSON.stringify(territory.demographics, null, 2) });
            }
        }
        let demographicsObj;
        try {
            demographicsObj = JSON.parse(formData.demographics);
        } catch (err) {
            setErrors({'demographics': 'must be valid JSON e.g. ' + demographicsPlaceholder});
            return;
        }

        const submissionData = {
            ...territory, // Includes the ID if we are editing
            name: formData.name,
            zipCodes: formData.zipCodes.split(',').map(zip => zip.trim()).filter(zip => zip),
            demographics: demographicsObj,
        };

        try {
            await onSave(submissionData);
        } catch (validationErrors) {
            setErrors(validationErrors);
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
                            placeholder="e.g., Northern Utah County" // <-- Add placeholder
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
                            placeholder="e.g., 84005, 84043, 84045" // <-- Add placeholder
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
                            placeholder={demographicsPlaceholder} // <-- Add placeholder
                            rows="5"
                        />
                    </div>
                    {errors.demographics && <p className="error-message">{errors.demographics}</p>}
                    <div className="form-actions">
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" className="btn" onClick={onCancel}>Cancel</button>
                    </div>
                </form>
            </div>
        </div>
    );
}