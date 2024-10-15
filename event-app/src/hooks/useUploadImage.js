import { useState } from 'react';
import axios from 'axios';

const useFileUpload = (eventId, token) => {
    const [uploading, setUploading] = useState(false);

    const handleFileUpload = async (selectedFile, setEvent) => {
        const formData = new FormData();
        formData.append('file', selectedFile);
        setUploading(true);
        try {
            const response = await axios.post(`${process.env.REACT_APP_API_BASE_URL}/Event/UploadImage/${eventId}`, formData, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'multipart/form-data'
                }
            });
            setEvent(prevEvent => ({ ...prevEvent, image: response.data.imageUrl }));
            alert('Image uploaded successfully!');
            window.location.reload();
        } catch (error) {
            console.error('Error uploading image:', error);
            alert('An error occurred during image upload.');
        }
    };

    return { handleFileUpload, uploading };
};

export default useFileUpload;
