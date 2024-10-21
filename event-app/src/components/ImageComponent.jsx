import React from 'react';

const ImageComponent = ({ imageData }) => {
    let fileType = imageData.charAt(0);
    let imageSrc;

    if (!imageData) return null;

    if (fileType === '/') {
        imageSrc = `data:image/jpg;base64,${imageData}`;
    } else if (fileType === 'i') {
        imageSrc = `data:image/png;base64,${imageData}`;
    }

    return <img src={imageSrc} alt="Event" className='event-image' />;
};

export default ImageComponent;