import React from 'react'
import ReactDOM from 'react-dom/client'
import { useEffect, useState } from 'react';
import { checkTokenExpiration } from '../authhelpers'


function shoonya() {


    const [formData, setFormData] = useState({
        
        Uid: '',
        Password: '',
        AuthSecreteKey: '',
        Imei: '',
        Vc: '',
        ApiKey: '',
        IsActive: true
    });
    const [apistatus, setApiStatus] = useState("");

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };
    const { status, user } = checkTokenExpiration();

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (status) {
            try {
                const response = await fetch('api/ShoonyaCredentials/add', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${user.token}`
                    },
                    body: JSON.stringify(formData),
                });
                const data = await response.json();

                if (response.ok) {
                    // Registration successful

                    setApiStatus(data.message);
                    console.log('User registered successfully!');
                } else {
                    setApiStatus('Error: ' + data.message);
                    console.error('Error:', data.message);
                    throw new Error(data.message);
                }
            } catch (error) {
                setApiStatus('Error during adding shoonya credentials:', error);
                console.error('Error during adding shoonya credentials:', error);
            }
        } else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }
    };




    return (
        /* < !--sign up section-- >*/
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-1">
                        <div className="heading_container">
                            <h2>
                                Add Stock Broker
                            </h2>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-6 col-md-11 offset-md-1">
                        <div className="form_container">
                            <form onSubmit={handleSubmit} type="submit">
                               
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

                                <div className="btn_box">
                                    <button>
                                        Add
                                    </button>
                                </div>
                                <div className="second-div">
                                    <label>Status: {apistatus}</label>
                                </div>
                            </form>
                        </div>
                    </div>

                </div>
            </div>
        </section>
        /*  <!--end sign up section-- >*/
    );
}
export default shoonya;