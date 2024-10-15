import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';
import "../styles/search.css";

export default function SearchForm({ events }) {
    const [myOptions, setMyOptions] = useState([]);
    const [selectedValue, setSelectedValue] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        getDataFromAPI();
    }, [events]);

    const getDataFromAPI = async () => {
        try {
            const optionsEvent = events.map(event => ({ id: event.id, name: event.name }));
            setMyOptions(optionsEvent);
        } catch (error) {
            console.error("Error fetching data: ", error);
            alert('An error occurred during fetching data. Please try again.');
        }
    };

    const handleSearchClick = () => {
        if (selectedValue) {
            navigate(`/${selectedValue.id}`);
        } else {
            alert('Please select an event.');
        }
    };

    return (
        <form className="search-window">
            <div className='react-search'>
                <Autocomplete
                    className='autocomplete'
                    freeSolo
                    autoComplete
                    autoHighlight
                    options={myOptions}
                    getOptionLabel={(option) => option.name}
                    onChange={(event, newValue) => setSelectedValue(newValue)}
                    renderInput={(params) => (
                        <TextField
                            {...params}
                            variant="outlined"
                            label="Enter events name"
                        />
                    )}
                />
                <button type="button" onClick={handleSearchClick} className='search-but'>
                    <i className="icon-magnifying" aria-hidden="true"></i>
                </button>
            </div>
        </form>
    );
}
