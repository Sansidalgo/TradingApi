import React, { useState, useEffect } from 'react';
import { checkTokenExpiration } from './AuthHelpers';

const DropDown = ({ apiPath, placeholder, displayProperty, valueProperty, onApiStatusChange, onSelectedOptionChange, selectedItemId }) => {
    const [options, setOptions] = useState([]);
    const [selectedOption, setSelectedOption] = useState('');
    const [apiStatus, setApiStatus] = useState('');
    const [selectedItemIdLocal, setSelectedItemIdLocal] = useState(0);

    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        if (status) {
            populateData(user.token);
        } else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }
    }, []);

    useEffect(() => {
        setSelectedItemIdLocal(selectedItemId);
    }, [selectedItemId]);

    useEffect(() => {
        if (selectedItemIdLocal && valueProperty) {
            const selectedOptionData = options.find(option => option.id === selectedItemIdLocal);
            if (selectedOptionData) {
                setSelectedOption(selectedOptionData[valueProperty]);
            }
        }
    }, [selectedItemIdLocal, valueProperty, options]);

    const handleSelectChange = (event) => {
        const newSelectedOption = event.target.value;
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
        
        <select value={selectedOption} onChange={handleSelectChange}>
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

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        if (response.ok) {
            const data = await response.json();
            console.log(data);
            if (data.status === 1) {
                console.log("options list");
                console.log(data.result);
                setOptions(data.result);
                setApiStatus(data.message);
            } else if (data.message && data.message.includes("error")) {
                setApiStatus('Error: ' + data.message);
                console.error('Error:', data.message);
            } else {
                setApiStatus(data.message);
            }
        } else {
            setApiStatus('Login failed!');
            console.error('Login failed');
            throw new Error('Login failed');
        }
    }
};

export default DropDown;
