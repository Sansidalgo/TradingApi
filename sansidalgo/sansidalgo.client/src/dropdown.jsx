import React, { useState, useEffect } from 'react';
import { checkTokenExpiration } from './authhelpers';

const Dropdown = ({ apiPath,  placeholder, displayProperty, valueProperty, onApiStatusChange, onSelectedOptionChange }) => {
    const [options, setOptions] = useState([]);
    const [selectedOption, setSelectedOption] = useState('');
    const [apiStatus, setApiStatus] = useState('');

    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        if (status) {
           
                populateData(user.token);
            
           
        } else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }
    }, []);

    const handleSelectChange = (event) => {
        const newSelectedOption = event.target.value;
        console.log(newSelectedOption);
        setSelectedOption(newSelectedOption);
        // Call the callback function with the updated selectedOption
        onSelectedOptionChange(newSelectedOption);
    };

    const handleApiStatusChangeLocal = (newApiStatus) => {
        // Call the callback function with the updated apiStatus
        onApiStatusChange(newApiStatus);
    };

    useEffect(() => {
        handleApiStatusChangeLocal(apiStatus);
    }, [apiStatus]);

    return (
        
            <select  value={selectedOption} onChange={handleSelectChange}>
                <option value="">{placeholder}</option>
                {options?.map((option) => (
                    <option key={option.id} value={option[valueProperty]}>
                        {option[displayProperty]}
                    </option>
                ))}
            </select>
            
  
    );

    async function populateData(token) {
        const response = await fetch(apiPath, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        const data = await response.json();

        console.log(data);
        if (response.ok) {
            if (data.status === 1) {
                setOptions(data.result);
                setApiStatus(data.message);
            } else if (data.message && data.message.includes("error")) {
                setApiStatus('Error: ' + data.message);
                console.error('Error:', data.message);
            }
            else {
                setApiStatus(data.message);
                
            }
        } else {
            setApiStatus('Login failed!');
            console.error('Login failed');
            throw new Error('Login failed');
        }
    }
};

export default Dropdown;
