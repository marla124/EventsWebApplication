import React, { useState } from 'react';
import Modal from 'react-modal';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import useCategory from '../hooks/useCategory';
import '../styles/create.css';
import BasicDateTimePicker from './BasicDateTimePicker';
import dayjs from 'dayjs';

export default function UpdateEventForm({ isOpen, onRequestClose, currentImage }) {
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [dateAndTime, setDateAndTime] = useState(dayjs(''));
    const [maxNumberOfPeople, setMaxNumberOfPeople] = useState('');
    const [address, setAddress] = useState('');
    const categories = useCategory();
    const eventId = window.location.href.split('/').pop();
    const [categoryId, setCategoryId] = useState('');
    const token = localStorage.getItem('jwtToken');
    const [image, setImage] = useState(currentImage);

    const handleUpdateEvent = async () => {
        try {
            const response = await axios.patch(process.env.REACT_APP_API_BASE_URL + '/Event/UpdateEvent', {
                id: eventId,
                name: name || null,
                description: description || null,
                dateAndTime: dateAndTime.isValid() ? dateAndTime.toISOString() : null,
                maxNumberOfPeople: maxNumberOfPeople || null,
                address: address || null,
                categoryId: categoryId || null,
                image: image || null,
            }, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Accept': 'application/json',
                },
            });

            console.log('update success', response.data);
            alert('update success');
            window.location.reload();
        } catch (error) {
            console.error('update error', error);
            alert('An error occurred while updating the event. Please try again.');
        }
    };

    return (
        <div className="create-event-window">
            <Modal isOpen={isOpen} onRequestClose={onRequestClose} className="react-modal-create">
                <h2 className='title-text-create-event'>Update event</h2>
                <input type="text" className='name-form' placeholder="Enter the name of the event" value={name} onChange={(e) => setName(e.target.value)} />
                <textarea type="text" className='description-form' placeholder="Enter the description of the event" value={description} onChange={(e) => setDescription(e.target.value)} />
                <input type="text" className='address-form' placeholder="Enter the address of the event" value={address} onChange={(e) => setAddress(e.target.value)} />
                <input type="number" className='number-form' placeholder="Enter the number of people of the event" value={maxNumberOfPeople} onChange={(e) => setMaxNumberOfPeople(e.target.value)} />
                <BasicDateTimePicker value={dateAndTime} onChange={(newValue) => setDateAndTime(dayjs(newValue))} />
                <select className='category-select' value={categoryId} onChange={(e) => {
                    setCategoryId(e.target.value);
                }}>
                    <option value="" disabled>Category</option>
                    {categories.map((category) => (
                        <option key={category.id} value={category.id}>{category.name}</option>
                    ))}
                </select>
                <button className="button-form" onClick={handleUpdateEvent}>Update</button>
            </Modal>
        </div>
    );
}
