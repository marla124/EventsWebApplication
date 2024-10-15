import { useState, useEffect } from 'react';
import axios from 'axios';

export default function useEvent() {
    const token = localStorage.getItem('jwtToken');
    const [event, setEvent] = useState([]);
    const url = window.location.href;
    const eventId = url.split('/').pop();
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get(process.env.REACT_APP_API_BASE_URL + `/Event/GetById/${eventId}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Accept': 'application/json'
                    }
                });
                setEvent(response.data);
            } catch (error) {
                console.error('Ошибка при выполнении запроса', error);
            }
        };

        fetchData();
    }, [])

    return event;
}           