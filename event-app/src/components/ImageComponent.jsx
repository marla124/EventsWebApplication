import React from 'react';

const ImageComponent = ({ imageData }) => {
    if (!imageData) return null;
    const imageSrc = `data:image/jpg;base64,${imageData}`;

    return <img src={imageSrc} alt="Event" className='event-image' />;
};

export default ImageComponent;
