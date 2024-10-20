import { Link, useNavigate } from 'react-router-dom';
import "../styles/navbar.css";
import "../assets/mfglabs-iconset-master/css/mfglabs_iconset.css";
import React, { useState } from "react";
import axios from 'axios';
import useUserAuthentication from '../hooks/useUserAuthentication';
import useGetUserRole from '../hooks/useGetUserRole';
import CreateEventForm from './CreateEventForm';

export default function Navbar({ onLogout }) {
    const { isUserLoggedIn, setIsUserLoggedIn } = useUserAuthentication();
    const navigate = useNavigate();
    const userRole = useGetUserRole();
    const [modalIsOpen, setModalIsOpen] = useState(false);

    const handleLogout = async () => {
        const refreshToken = localStorage.getItem('refreshToken');
        if (refreshToken) {
            try {
                await axios.delete(`/api/Token/Revoke/${refreshToken}`, {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
                localStorage.removeItem('jwtToken');
                localStorage.removeItem('refreshToken');
                window.location.reload();
                setIsUserLoggedIn(false);
                onLogout();
                navigate('/');
            } catch (error) {
                console.error('Error revoking token:', error);
            }
        }
    };

    const handleButtonClick = () => {
        if (isUserLoggedIn) {
            setModalIsOpen(true);
        } else {
            navigate('/login');
        }
    };

    return (
        <div className="header_menu">
            <div className="logo">
                <Link to="/" className='logo-text'>
                    <h1>EventWebApp</h1>
                </Link>
            </div>
            <div className="button_items">
                {isUserLoggedIn ? (
                    <>
                        <button onClick={handleLogout} className="login_butt">
                            Log Out
                        </button>
                        <Link to="/myevents" className="login_butt">
                            My Events
                        </Link>
                    </>
                ) : (
                    <Link to="/login" className="login_butt">
                        Log In
                    </Link>
                )}
                {userRole.role === "Admin" && (
                    <>
                        <button className="login_butt" onClick={handleButtonClick}>
                            Create
                        </button>
                        <CreateEventForm isOpen={modalIsOpen} onRequestClose={() => setModalIsOpen(false)} />
                    </>
                )}
            </div>
        </div>
    );
}
