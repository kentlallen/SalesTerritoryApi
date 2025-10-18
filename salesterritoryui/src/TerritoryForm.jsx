// src/TerritoryForm.jsx

import { useState, useEffect } from 'react';

export function TerritoryForm({ territory, onSave, onCancel }) {
    const [formData, setFormData] = useState({
        name: '',
        zipCodes: '',
        demographics: '{}',
    });
    const [error, setError] = useState('');

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
            setFormData({ name: '', zipCodes: '', demographics: '{}' });
        }
    }, [territory]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        setError('');

        let demographicsObj;
        try {
            demographicsObj = JSON.parse(formData.demographics);
        } catch (err) {
            setError('Demographics must be valid JSON.');
            return;
        }

        const submissionData = {
            ...territory, // Includes the ID if we are editing
            name: formData.name,
            zipCodes: formData.zipCodes.split(',').map(zip => zip.trim()).filter(zip => zip),
            demographics: demographicsObj,
        };

        onSave(submissionData);
    };

    return (
        <div className="modal-backdrop">
            <div className="modal-content">
                <h2>{territory ? 'Edit Territory' : 'Add New Territory'}</h2>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="name">Territory Name</label>
                        <input type="text" id="name" name="name" value={formData.name} onChange={handleChange} required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="zipCodes">Zip Codes (comma-separated)</label>
                        <input type="text" id="zipCodes" name="zipCodes" value={formData.zipCodes} onChange={handleChange} required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="demographics">Demographics (JSON format)</label>
                        <textarea id="demographics" name="demographics" value={formData.demographics} onChange={handleChange} rows="5" />
                    </div>
                    {error && <p className="error-message">{error}</p>}
                    <div className="form-actions">
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" className="btn" onClick={onCancel}>Cancel</button>
                    </div>
                </form>
            </div>
        </div>
    );
}