import useEventsByCriteria from '../hooks/useEventsByCriteria';
import Navbar from './Navbar';
import React, { useState, useEffect } from 'react';
import "../styles/home.css";
import { Link } from 'react-router-dom';
import EventItem from './EventItem';
import Pagination from './Pagination';
import SearchForm from './SearchForm';
import BasicDatePicker from './BasicDatePicker';
import useCategory from '../hooks/useCategory'

export default function HomeForm() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const [recordsPerPage] = useState(10);
    const [selectedDate, setSelectedDate] = useState(null);
    const [selectedAddress, setSelectedAddress] = useState('');
    const [selectedCategory, setSelectedCategory] = useState('');
    const eventsFiltre = useEventsByCriteria(selectedDate, selectedAddress, selectedCategory);
    const category = useCategory('');
    useEffect(() => {
        if (eventsFiltre) {
            setData(eventsFiltre);
            setLoading(false);
        }
    }, [eventsFiltre]);

    const indexOfLastRecord = currentPage * recordsPerPage;
    const indexOfFirstRecord = indexOfLastRecord - recordsPerPage;
    const currentRecords = data.slice(indexOfFirstRecord, indexOfLastRecord);
    const nPages = Math.ceil(data.length / recordsPerPage);
    const uniqueAddresses = [...new Set(eventsFiltre.map(event => event.address))];

    return (
        <div className='page-container'>
            <Navbar />
            <div className='events-container'>
                <div className='events-list'>
                    <h1 className='title-header-events'>Events</h1>
                    <SearchForm events={eventsFiltre} />
                    <div className='filtre-container'>
                        <select
                            className='filtre-butt'
                            value={selectedAddress}
                            onChange={(e) => setSelectedAddress(e.target.value || null)}
                        >
                            <option value="">Address</option>
                            {uniqueAddresses.map((address, index) => (
                                <option key={index} value={address}>{address}</option>
                            ))}
                        </select>

                        <BasicDatePicker className='date'
                            value={selectedDate}
                            onChange={(newValue) => setSelectedDate(newValue)}
                        />
                        <select
                            className='filtre-butt'
                            value={selectedCategory}
                            onChange={(e) => setSelectedCategory(e.target.value)}
                        >
                            <option value="" disabled>Category</option>
                            {category.map((cat) => (
                                <option key={cat.id} value={cat.id}>{cat.name}</option>
                            ))}
                        </select>
                    </div>
                    <div className='list'>
                        {currentRecords.map(event => (
                            <Link to={`/${event.id}`} key={event.id}>
                                <EventItem event={event} />
                            </Link>
                        ))}
                    </div>
                </div>
                <Pagination
                    nPages={nPages}
                    currentPage={currentPage}
                    setCurrentPage={setCurrentPage}
                />
            </div>
        </div>
    );
}
