import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import HomeForm from './components/HomeForm.jsx'
import LoginForm from './components/LoginForm.jsx';
import RegistrationForm from './components/RegistrationForm.jsx';
import EventPage from './components/EventPage.jsx';
import axios from 'axios';
import MyEvents from './components/MyEvents.jsx';
axios.interceptors.response.use(
  response => response,
  async error => {
    const firstRequest = error.config;
    if (error.response.status == 401 && !firstRequest._retry) {
      firstRequest._retry = true;
      const newToken = await refreshToken();
      firstRequest.headers['Authorization'] = `Bearer ${newToken}`;
      return axios(firstRequest);
    }
  }
);

const refreshToken = async () => {
  try {
    const storedRefreshToken = localStorage.getItem('refreshToken');

    const response = await axios.post('/api/Auth/RefreshToken', {
      refreshToken: storedRefreshToken
    }, {
      headers: {
        'Content-Type': 'application/json'
      }
    });

    const { jwtToken, refreshToken } = response.data;
    localStorage.setItem('jwtToken', jwtToken);
    localStorage.setItem('refreshToken', refreshToken);

    return jwtToken;
  } catch (error) {
    console.error('Error updating token', error);
  }
};

const PrivateRoute = ({ element: Component, ...rest }) => {
  const token = localStorage.getItem('jwtToken');
  return token ? <Component {...rest} /> : <Navigate to="/login" />;
};

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<HomeForm />} />
        <Route path="/login" element={<LoginForm />} />
        <Route path="/:id" element={<EventPage />} />
        <Route path="/myevents" element={<PrivateRoute element={MyEvents} />} />
        <Route path="/signup" element={<RegistrationForm />} />
      </Routes></>
  );
}

export default App;
