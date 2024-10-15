import { useState, useEffect } from 'react';
import axios from 'axios';

export default function useGetUserRole() {
    const token = localStorage.getItem('jwtToken');
    const [role, setRole] = useState([]);
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get(process.env.REACT_APP_API_BASE_URL + '/User/GetUserRole', {
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