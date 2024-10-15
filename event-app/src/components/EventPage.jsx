import React, { useState, useEffect } from "react";
import axios from 'axios';
import { useNavigate } from "react-router-dom";
import Navbar from "./Navbar";
import UpdateEventForm from "./UpdateEventForm";
import { formatDate } from "../utils/formatDate";
import useUserAuthentication from "../hooks/useUserAuthentication";
import useGetUserRole from "../hooks/useGetUserRole";
import "../styles/eventPage.css";
import ImageComponent from "./ImageComponent";
import useFileUpload from "../hooks/useUploadImage";

export default function EventPage() {
    const [event, setEvent] = useState(null);
    const token = localStorage.getItem('jwtToken');
    const eventId = window.location.href.split('/').pop();
    const { isUserLoggedIn } = useUserAuthentication();
    const userRole = useGetUserRole();
    const navigate = useNavigate();
    const [selectedFile, setSelectedFile] = useState(null);
    const [modalIsOpen, setModalIsOpen] = useState(false);
    const { handleFileUpload, uploading } = useFileUpload(eventId, token);
    useEffect(() => {
        const fetchEvent = async () => {
            try {
                const response = await axios.get(`${process.env.REACT_APP_API_BASE_URL}/Event/GetById/${eventId}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Accept': 'application/json'
                    }
                });
                setEvent(response.data);
            } catch (error) {
                console.error('Error fetching event data:', error);
            }
        };

        fetchEvent();
    }, [eventId, token]);

    const handleRegistration = () => {
        axios.post(`${process.env.REACT_APP_API_BASE_URL}/Practicant/AddParticipantToEvent/${eventId}`, {}, {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Accept': 'application/json'
            }
        })
            .then(response => {
                console.log(response);
                if (response.status === 200) {
                    alert('You are a participant in the event!');
                }
            })
            .catch(error => {
                console.log(error);
                if (error.response) {
                    if (error.response.status === 409) {
                        alert('You have already registered for this event.');
                    } else if (error.response.status === 400) {
                        alert('There are no more places for this event');
                    } else {
                        alert('An error occurred during registration.');
                    }
                } else {
                    alert('An error occurred during registration.');
                }
            });
    };




    const handleDeleteEvent = async () => {
        try {
            await axios.delete(`${process.env.REACT_APP_API_BASE_URL}/Event/DeleteById/${eventId}`, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Accept': 'application/json'
                }
            });
            alert('Event deleted successfully!');
            navigate('/');
        } catch (error) {
            alert('An error occurred during event deletion.');
        }
    };

    const handleButtonClick = () => {
        if (isUserLoggedIn) {
            setModalIsOpen(true);
        } else {
            navigate('/login');
        }
    };

    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    if (!event) {
        return <div>Loading...</div>;
    }

    return (
        <div className='event-page-container'>
            <Navbar />
            <div className='event-details'>
                <h1 className='event-name'>{event.name}</h1>
                {event.image ? (
                    <ImageComponent imageData={event.image} className='event-image-onep' />
                ) : (
                    userRole.role === "Admin" && (
                        <div className='file-upload'>
                            <input
                                type="file"
                                id="fileInput"
                                onChange={handleFileChange}
                                style={{ display: 'none' }}
                            />
                            <label htmlFor="fileInput" className="login_butt">
                                Choose File
                            </label>
                            <button onClick={() => handleFileUpload(selectedFile, setEvent)} disabled={uploading} className="login_butt">
                                Upload
                            </button>
                        </div>
                    )
                )}

                <p className='event-description'>{event.description}</p>
                <div className='event-info'>
                    <p><strong>Date and time:</strong> {formatDate(event.dateAndTime)}</p>
                    {event.address ? <p><strong>Address:</strong> {event.address}</p> : ""}
                    <p><strong>Number of places:</strong> {event.maxNumberOfPeople}</p>
                </div>
                {isUserLoggedIn ? <button className="button-reg" onClick={handleRegistration}>Register for the event</button> : ""}
                {userRole.role === "Admin" && (
                    <div className="butt">
                        <button className="login_butt" onClick={handleDeleteEvent}>Delete</button>
                        <button className="login_butt" onClick={handleButtonClick}>Update</button>
                        <UpdateEventForm isOpen={modalIsOpen} onRequestClose={() => setModalIsOpen(false)} currentImage={event?.image} />
                    </div>
                )}
            </div>
        </div>
    );
}
