import React from 'react';
import "../styles/eventItem.css";
import { formatDate } from "../utils/formatDate";
import ImageComponent from './ImageComponent';

export default function EventItem(props) {
  return (
    <div className='event-item'>
      {props.event.image ? <ImageComponent imageData={props.event.image} className='event-image' /> : ""}
      <p className='name-event'>{props.event.name}</p>
      {props.event.dateAndTime ? <p className='date-event'>{formatDate(props.event.dateAndTime)}</p> : ""}
      {props.event.address ? <p className='address-event'>Address: {props.event.address}</p> : ""}
      <p className='num-event'>Number of places: {props.event.maxNumberOfPeople}</p>
    </div>
  );
}
