import { useState, useEffect } from 'react';
import axios from 'axios';

const useEvents = (date, address, categoryId) => {
    const [events, setEvents] = useState([]);

    useEffect(() => {
        const fetchEvents = async () => {
            try {
                const response = await axios.get(process.env.REACT_APP_API_BASE_URL + '/Event/GetEventsByCriteria', {
                    params: { date, address, categoryId }
                });
                const sortedEvents = response.data.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
                setEvents(sortedEvents);
            } catch (error) {
                console.error('Error fetching events:', error);
            }
        };

        fetchEvents();
    }, [date, address, categoryId]);

    return events;
};

export default useEvents;