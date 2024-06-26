import React, { useState, useEffect } from 'react';

function Credentials({ onFormChange, initialValues }) {
    const [formData, setFormData] = useState({
        id: 0,
        name: '',
        uid: '',
        password: '',
        authSecreteKey: '',
        imei: '',
        vc: '',
        apiKey: '',
        isActive: true,
    });

    const [formErrors, setFormErrors] = useState({
        credentialName: '*',
    });

    useEffect(() => {
        setFormData(initialValues);
    }, [initialValues]);

    const handleChange = (e) => {
        const { name, value, type } = e.target;

        // Handle password field separately
        const newValue = type === 'password' ? e.target.value : value;

        setFormData({ ...formData, [name]: newValue });
        setFormErrors({ ...formErrors, credentialName: '*' });
    };

    useEffect(() => {
        onFormChange(formData);
    }, [formData]);

    return (
        <div className="boxTypeDiv">
            <div>
                <div style={{ display: 'flex', flexDirection: 'row' }}>
                    <label>Credentials Name:</label>
                    <div style={{ color: 'red' }}>{formErrors.credentialName}</div>
                </div>
                <input
                    name="name"
                    value={formData.name}
                    onChange={handleChange}
                    type="text"
                    placeholder="Enter your name"
                />
                <input name="id" value={formData.id} onChange={handleChange} type="hidden" />
            </div>
            <div>
                <label>UID:</label>
                <input
                    name="uid"
                    value={formData.uid}
                    onChange={handleChange}
                    type="text"
                    placeholder="Enter your UID"
                />
            </div>
            <div>
                <label>Auth Secret Key:</label>
                <input
                    name="authSecreteKey"
                    value={formData.authSecreteKey}
                    onChange={handleChange}
                    type="text"
                    placeholder="Enter your auth secret key"
                />
            </div>
            <div>
                <label>IMEI:</label>
                <input name="imei" value={formData.imei} onChange={handleChange} type="text" placeholder="Enter your IMEI" />
            </div>
            <div>
                <label>VC:</label>
                <input name="vc" value={formData.vc} onChange={handleChange} type="text" placeholder="Enter your VC" />
            </div>
            <div>
                <label>API Key:</label>
                <input
                    name="apiKey"
                    value={formData.apiKey}
                    onChange={handleChange}
                    type="text"
                    placeholder="Enter your API key"
                />
            </div>
            <div>
                <label>Is Active:</label>
                <input
                    name="isActive"
                    value={formData.isActive}
                    onChange={handleChange}
                    type="text"
                    placeholder="Enter your isActive"
                />
            </div>
            <div>
                <label>Password:</label>
                <input
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    type="password"
                    placeholder="Enter your password"
                />
            </div>
        </div>
    );
}

export default Credentials;
