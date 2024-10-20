import { useState, useEffect } from 'react';
import axios from 'axios';

export default function useEvent() {
    const token = localStorage.getItem('jwtToken');
    const [category, setCategory] = useState([]);
    const url = window.location.href;
    const eventId = url.split('/').pop();
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get('/api/Event/GetCategory', {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Accept': 'application/json'
                    }
                });
                setCategory(response.data);
            } catch (error) {
                console.error('Ошибка при выполнении запроса', error);
            }
        };

        fetchData();
    }, [])

    return category;
}           