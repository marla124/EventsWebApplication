import { useState, useEffect } from 'react';
import axios from 'axios';

export default function useGetUserRole() {
    const token = localStorage.getItem('jwtToken');
    const [role, setRole] = useState([]);
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get('/api/User/GetUserRole', {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Accept': 'application/json'
                    }
                });
                setRole(response.data);
            } catch (error) {
                console.error('Ошибка при выполнении запроса', error);
            }
        };

        fetchData();
    }, [])

    return role;
}           