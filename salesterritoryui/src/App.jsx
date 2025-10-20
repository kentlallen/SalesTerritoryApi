// Main application component - handles state management and API communication
import { useState, useEffect } from 'react';
import { TerritoryForm } from './TerritoryForm';
import { TerritoryDetailsModal } from './TerritoryDetailsModal';
import './App.css';

const API_URL = 'https://localhost:7004/api/Territories';

function App() {
    const [territories, setTerritories] = useState([]);
    const [isFormModalOpen, setIsFormModalOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const [editingTerritory, setEditingTerritory] = useState(null);
    const [viewingTerritory, setViewingTerritory] = useState(null);
    const [networkError, setNetworkError] = useState(null);

    useEffect(() => {
        fetchTerritories();
    }, []);

    const fetchTerritories = async () => {
        setIsLoading(true);
        try {
            const response = await fetch(API_URL);
            if (!response.ok) throw new Error("Network response was not ok");
            const data = await response.json();
            setTerritories(data);
        } catch (error) {
            console.error('Failed to fetch territories:', error);
            // Distinguish between network errors and other failures
            if (error instanceof TypeError) {
                setNetworkError("Could not connect to the API. Please ensure the server is running and accessible.");
            } else {
                setNetworkError("An unexpected error occurred while fetching data.");
            }
        } finally {
            setIsLoading(false);
        }
    };

    const handleAddNew = () => {
        setEditingTerritory(null);
        setIsFormModalOpen(true);
    };

    const handleEdit = (territory) => {
        setEditingTerritory(territory);
        setIsFormModalOpen(true);
    };

    const handleViewDetails = (territory) => {
        setViewingTerritory(territory);
    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this territory?')) {
            try {
                const response = await fetch(`${API_URL}/${id}`, {
                    method: 'DELETE',
                });
                if (!response.ok) throw new Error("Failed to delete");
                fetchTerritories();
            } catch (error) {
                console.error('Failed to delete territory:', error);
            }
        }
    };

    const handleSave = async (territory) => {
        const isEditing = !!territory.id;
        const url = isEditing ? `${API_URL}/${territory.id}` : API_URL;
        const method = isEditing ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(territory),
        });

        if (!response.ok) {
            if (response.status === 400) {
                const errorData = await response.json();
                // Transform API validation errors into a format the form can use
                const formattedErrors = {};
                for (const key in errorData.errors) {
                    const lowerKey = key.toLowerCase();
                    const messages = errorData.errors[key];

                    // Handle array validation errors (like ZipCodes[0])
                    if (lowerKey.startsWith('zipcodes[')) {
                        if (formattedErrors.zipcodes) {
                            formattedErrors.zipcodes += ` ${messages.join(' ')}`;
                        } else {
                            formattedErrors.zipcodes = messages.join(' ');
                        }
                    } else {
                        formattedErrors[lowerKey] = messages.join(' ');
                    }
                }
                throw formattedErrors;
            } else {
                // Handle server errors (500, 503, etc.)
                const errorJson = await response.json().catch(() => null);
                const message = errorJson?.detail || "An unexpected server error occurred."
                throw new Error(message);
            }
        }

        setIsFormModalOpen(false);
        setEditingTerritory(null);
        fetchTerritories();
    };


    return (
        <div className="app-container">
            {networkError && (
                <div className="network-error-banner">
                    <p><strong>Connection Error:</strong> {networkError}</p>
                    <button className="btn btn-primary" onClick={fetchTerritories}>Retry</button>
                </div>
            )}
            <header>
                <h1>Sales Territory Manager</h1>
                <button className="btn btn-primary" onClick={handleAddNew}>+ Add New Territory</button>
            </header>
            <main>
                {isLoading && (
                    <div className="loading-indicator">
                        <div className="spinner"></div>
                        <p>Loading territories...</p>
                    </div>
                )}
                {!isLoading && !networkError && (
                    <div className="territory-list">
                        <table>
                            <thead>
                                <tr>
                                    <th>Territory ID</th>
                                    <th>Territory Name</th>
                                    <th>Zip Codes</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            {!isLoading && territories.length < 1 && (<tbody className='no-territories'>No territories found. Add one now.</tbody>)}
                            <tbody>
                                {territories.map(t => (
                                    <tr key={t.id}>
                                        <td>{t.id}</td>
                                        <td>{t.name}</td>
                                        <td>{t.zipCodes.join(', ')}</td>
                                        <td className="actions">
                                            {/* New "View" button */}
                                            <button className="btn btn-secondary" onClick={() => handleViewDetails(t)}>View</button>
                                            <button className="btn" onClick={() => handleEdit(t)}>Edit</button>
                                            <button className="btn btn-danger" onClick={() => handleDelete(t.id)}>Delete</button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </main>

            {/* Form modal for create/edit operations */}
            {isFormModalOpen && (
                <TerritoryForm
                    territory={editingTerritory}
                    onSave={handleSave}
                    onCancel={() => {
                        setIsFormModalOpen(false);
                        setEditingTerritory(null);
                    }}
                />
            )}

            {/* Details modal for viewing territory information */}
            <TerritoryDetailsModal
                territory={viewingTerritory}
                onClose={() => setViewingTerritory(null)}
            />
        </div>
    );
}

export default App;