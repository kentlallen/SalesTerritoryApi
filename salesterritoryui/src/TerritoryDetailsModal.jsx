// Modal component for displaying territory details in read-only mode
export function TerritoryDetailsModal({ territory, onClose }) {
    if (!territory) {
        return null;
    }

    return (
        <div className="modal-backdrop" onClick={onClose}>
            <div className="modal-content" onClick={e => e.stopPropagation()}>
                <h2>{territory.name}</h2>
                <div className="details-section">
                    <strong>Zip Codes:</strong>
                    <p>{territory.zipCodes.join(', ')}</p>
                </div>
                <div className="details-section">
                    <strong>Demographics:</strong>
                    {Object.entries(territory.demographics).length > 0 ? (
                        <ul className="demographics-list">
                            {Object.entries(territory.demographics).map(([key, value]) => (
                                <li key={key}>
                                    <span className="key">{key}:</span>
                                    <span className="value">{JSON.stringify(value)}</span>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>No demographic data available.</p>
                    )}
                </div>
                <div className="form-actions">
                    <button type="button" className="btn" onClick={onClose}>Close</button>
                </div>
            </div>
        </div>
    );
}