import { useState, useEffect } from 'react';
import axios from 'axios';

export default function useEvents() {
    const [events, setEvents] = useState([]);
    const token = localStorage.getItem('jwtToken');

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get('/api/Event/GetEvents', {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Accept': 'application/json'
                    }
                });
                console.log('Fetched events:', response.data);
                const sortedEvents = response.data.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
                setEvents(sortedEvents);
            } catch (error) {
                console.error('Ошибка при выполнении запроса', error);
            }
        };

        fetchData();
    }, [token]);

    return events;
}
