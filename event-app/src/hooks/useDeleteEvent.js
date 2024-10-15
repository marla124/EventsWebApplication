import { useState, useEffect } from 'react';
import axios from 'axios';

export default function useDeleteEvent() {
    const token = localStorage.getItem('jwtToken');
    const [event, setEvent] = useState(null);
    const [error, setError] = useState(null);
    const url = window.location.href;
    const eventId = url.split('/').pop();

    useEffect(() => {
        const deleteEvent = async () => {
            try {
                const response = await axios.delete(`${process.env.REACT_APP_API_BASE_URL}/Event/DeleteById/${eventId}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Accept': 'application/json'
                    }
                });
                setEvent(response.data);
            } catch (error) {
                console.error('delete error', error);
                setError(error);
            }
        };

        deleteEvent();
    }, [eventId, token]);

    return { event, error };
}
