import React, { useState } from 'react';
import Navbar from './Navbar';
import "../styles/login.css"
import { Link, useNavigate } from 'react-router-dom';
import axios from "axios";

export default function LoginForm() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    const data = { email, password };

    try {
      const response = await axios.post("/api/Auth/Login", data);
      if (response.status === 201 || response.status === 200) {
        const { jwtToken, refreshToken } = response.data;
        localStorage.setItem('jwtToken', jwtToken);
        localStorage.setItem('refreshToken', refreshToken);
        navigate('/');
      }
    } catch (error) {
      console.error('Error generating token:', error);
      alert('An error occurred while logging in to your account. Please try again.');
    }
  };

  return (
    <div className="block">
      <Navbar />
      <div className="login-container">
        <div className="login form">
          <header>Login</header>
          <form onSubmit={handleSubmit}>
            <input type="email" placeholder="Enter your email" value={email} onChange={(e) => setEmail(e.target.value)} required />
            <input type="password" placeholder="Enter your password" value={password} onChange={(e) => setPassword(e.target.value)} required minLength="8" maxLength="64" />
            <input type="submit" className="button" value="Login" />
          </form>
          <div className="signup">
            <span className="signup">Don't have an account?
              <Link to="/signup"><label htmlFor="check"> Signup</label></Link>
            </span>
          </div>
        </div>
      </div>
    </div>
  );
}
