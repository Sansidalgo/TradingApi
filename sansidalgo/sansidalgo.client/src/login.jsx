import React from 'react'
import ReactDOM from 'react-dom/client'
import {  Link } from 'react-router-dom';
import { useEffect, useState } from 'react';



const login = ({ handleSuccessfulLogin }) => {
    

   
    const [formData, setFormData] = useState({
        Name:'',
        EmailId: '',
        PhoneNo: '',
        Password: '',
    });
    const [apistatus, setApiStatus] = useState("");

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('login/SignIn', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });
            const data = await response.json(); // Call this only once

            if (response.ok) {
                // Registration successful
                if (data.error) {
                    console.error('Error:', data.error);
                    setApiStatus('Error: ' + data.error);
                    // Handle the error, display a message, or redirect the user
                } else if (data.token) {
                    console.log('User Logged in successfully!');
                    setApiStatus('User Logged in successfully!');
                    // Assuming a successful login sets loggedIn to true
                    localStorage.setItem('token', data.token);
                    handleSuccessfulLogin();
                } else {
                    console.log('Unexpected response:', data);
                }
            } else {
                // Registration failed
                setApiStatus('Login failed!');
                console.error('Login failed');
            }
        } catch (error) {
            setApiStatus('Error during Login')
            console.error('Error during Login:', error);
        }
    };



    return ( /*< !--login section-- >*/
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-1">
                        <div className="heading_container">
                            <h2>
                                Login
                            </h2>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-6 col-md-11 offset-md-1">
                        <div className="form_container">
                            <form onSubmit={handleSubmit}>
                                <div>
                                    <input name="EmailId" value={formData.EmailId} onChange={handleChange} type="email" placeholder="Email Id" />
                                </div>
                                <div>
                                    <input type="text" placeholder="OR" disabled />
                                </div>
                                <div>
                                    <input name="PhoneNo" value={formData.PhoneNo} onChange={handleChange} type="text" placeholder="Phone Number" />
                                </div>
                                <div>
                                    <input name="Password" value={formData.Password} onChange={handleChange} type="password" placeholder="password" />
                                </div>

                                <div className="btn_box" >
                                    <button type="submit">
                                        Login
                                    </button>
                                </div>
                                <div className="second-div">
                                    <label>Status: {apistatus}</label>
                                </div>
                               
                                
                            </form>
                            <div className="second-div">

                                <Link to="/signup">Go to Signup</Link>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>
        /*  <!--end login section-- >*/
    );
}
export default login;