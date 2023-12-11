import React from 'react'
import { useState } from 'react';

function credentials({ onFormChange }) {


    const [formData, setFormData] = useState({
        Name: '',
        Uid: '',
        Password: '',
        AuthSecreteKey: '',
        Imei: '',
        Vc: '',
        ApiKey: '',
        IsActive: true
    });
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
        onFormChange(formData);
    };
    return (
        <div className="boxTypeDiv" >

            <div>
                <input name="Name" value={formData.Name} onChange={handleChange} type="text" placeholder="Name" />
            </div>
            <div>
                <input name="Uid" value={formData.Uid} onChange={handleChange} type="text" placeholder="Uid" />
            </div>
            <div>
                <input name="AuthSecreteKey" value={formData.AuthSecreteKey} onChange={handleChange} type="text" placeholder="AuthSecreteKey" />
            </div>
            <div>
                <input name="Imei" value={formData.Imei} onChange={handleChange} type="text" placeholder="Imei" />
            </div>
            <div>
                <input name="Vc" value={formData.Vc} onChange={handleChange} type="text" placeholder="Vc" />
            </div>

            <div>
                <input name="ApiKey" value={formData.ApiKey} onChange={handleChange} type="text" placeholder="ApiKey" />
            </div>
            <div>
                <input name="IsActive" value={formData.IsActive} onChange={handleChange} type="text" placeholder="IsActive" />
            </div>

            <div>
                <input name="Password" value={formData.Password} onChange={handleChange} type="password" placeholder="password" />
            </div>

        </div>
    );
}
export default credentials;