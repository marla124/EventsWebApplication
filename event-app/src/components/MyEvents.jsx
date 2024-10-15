import useMyEvents from "../hooks/useMyEvents";
import Navbar from './Navbar';
import React, { useState, useEffect } from 'react';
import "../styles/home.css";
import { Link } from 'react-router-dom';
import Pagination from './Pagination';
import EventItem from './EventItem';

export default function MyEvents() {
    const events = useMyEvents();
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const [recordsPerPage] = useState(10);

    useEffect(() => {
        if (events) {
            setData(events);
            setLoading(false);
        }
    }, [events]);
    const indexOfLastRecord = currentPage * recordsPerPage;
    const indexOfFirstRecord = indexOfLastRecord - recordsPerPage;
    const currentRecords = data.slice(indexOfFirstRecord, indexOfLastRecord);
    const nPages = Math.ceil(data.length / recordsPerPage);

    return (
        <div className='page-container'>
            <Navbar />
            <div className='events-container'>
                <div className='events-list'>
                    <h1 className='title-header-events'>Events</h1>
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