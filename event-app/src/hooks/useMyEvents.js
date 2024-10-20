import { useState, useEffect } from 'react';
import axios from 'axios';

export default function useMyEvents() {
    const [events, setEvents] = useState([]);
    const token = localStorage.getItem('jwtToken');

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get('/api/Event/GetUsersEvents', {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Accept': 'application/json'
                    }
                });
                const sortedEvents = response.data.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
                setEvents(sortedEvents);
            } catch (error) {
                console.error('Ошибка при выполнении запроса', error);
            }
        };

        fetchData();
    }, [])
    if (!events) {
        return <p>Loading...</p>;
    }

    return events;
}