import React from 'react'
import { useState, useEffect } from 'react';

function Credentials({ onFormChange,initialValues }) {


    const [formData, setFormData] = useState({
        name: '',
        uid: '',
        password: '',
        authSecreteKey: '',
        imei: '',
        vc: '',
        apiKey: '',
        isActive: true,
       
    });
    useEffect(() => {
    
        setFormData(initialValues)
    }, [initialValues]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
        onFormChange(formData);
    };
    return (
        <div className="boxTypeDiv">
            <div>
                <label >Name:</label>
                <input name="name" value={formData.name} onChange={handleChange} type="text" placeholder="Enter your name" />
            </div>
            <div>
                <label >UID:</label>
                <input name="uid" value={formData.uid} onChange={handleChange} type="text" placeholder="Enter your UID" />
            </div>
            <div>
                <label >Auth Secret Key:</label>
                <input name="authSecreteKey" value={formData.authSecreteKey} onChange={handleChange} type="text" placeholder="Enter your auth secret key" />
            </div>
            <div>
                <label >IMEI:</label>
                <input name="imei" value={formData.imei} onChange={handleChange} type="text" placeholder="Enter your IMEI" />
            </div>
            <div>
                <label >VC:</label>
                <input name="vc" value={formData.vc} onChange={handleChange} type="text" placeholder="Enter your VC" />
            </div>
            <div>
                <label >API Key:</label>
                <input name="apiKey" value={formData.apiKey} onChange={handleChange} type="text" placeholder="Enter your API key" />
            </div>
            <div>
                <label >Is Active:</label>
                <input name="isActive" value={formData.isActive} onChange={handleChange} type="text" placeholder="Enter your isActive" />
            </div>
            <div>
                <label >Password:</label>
                <input name="password" value={formData.password} onChange={handleChange} type="password" placeholder="Enter your password" />
            </div>
        </div>
    );

}
export default Credentials;