// src/App.jsx

import { useState, useEffect } from 'react';
import { TerritoryForm } from './TerritoryForm';
import { TerritoryDetailsModal } from './TerritoryDetailsModal'; // <-- Import the new component
import './App.css';

const API_URL = 'https://localhost:7004/Territories'; // <-- IMPORTANT: Verify your API's port!

function App() {
    const [territories, setTerritories] = useState([]);
    const [isFormModalOpen, setIsFormModalOpen] = useState(false);
    const [editingTerritory, setEditingTerritory] = useState(null);
    const [viewingTerritory, setViewingTerritory] = useState(null); // <-- New state for the details modal

    useEffect(() => {
        fetchTerritories();
    }, []);

    const fetchTerritories = async () => {
        try {
            const response = await fetch(API_URL);
            if (!response.ok) throw new Error("Network response was not ok");
            const data = await response.json();
            setTerritories(data);
        } catch (error) {
            console.error('Failed to fetch territories:', error);
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

    // Function to open the details modal
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
                    // Re-format the API error response to be easier to use in the form
                    const formattedErrors = {};
                    for (const key in errorData.errors) {
                        const lowerKey = key.toLowerCase();
                        const messages = errorData.errors[key];

                        // Check if this is a ZipCodes error (e.g., "ZipCodes[0]")
                        if (lowerKey.startsWith('zipcodes[')) {
                            // If so, aggregate all zip code messages under a single 'zipcodes' key
                            if (formattedErrors.zipcodes) {
                                formattedErrors.zipcodes += ` ${messages.join(' ')}`;
                            } else {
                                formattedErrors.zipcodes = messages.join(' ');
                            }
                        } else {
                            // Otherwise, handle it as a normal field (like 'Name')
                            formattedErrors[lowerKey] = messages.join(' ');
                        }
                    }
                    throw formattedErrors;
                }
                throw new Error("Failed to save territory");
            }

            setIsFormModalOpen(false);
            setEditingTerritory(null);
            fetchTerritories();
    };


    return (
        <div className="app-container">
            <header>
                <h1>Vivint Sales Territory Manager</h1>
                <button className="btn btn-primary" onClick={handleAddNew}>+ Add New Territory</button>
            </header>
            <main>
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
            </main>

            {/* Render the Form Modal */}
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

            {/* Render the Details Modal */}
            <TerritoryDetailsModal
                territory={viewingTerritory}
                onClose={() => setViewingTerritory(null)}
            />
        </div>
    );
}

export default App;