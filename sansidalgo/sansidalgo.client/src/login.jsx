import React, { useState } from 'react';
import { Link } from 'react-router-dom';

const Login = ({ handleSuccessfulLogin }) => {
    const [formData, setFormData] = useState({

        EmailId: '',
        PhoneNo: '',
        Password: '',
    });
    const [apistatus, setApiStatus] = useState('');

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSuccessfulLoginLocal = async () => {
        try {
            // Perform your login logic...

            // Assuming the server returns user information during login
            // Replace with your actual logic
            const userData = await fetchUserData(); // Assuming this function fetches user data
            localStorage.setItem('userData', JSON.stringify(userData));
            // Update the user information in the state or context
            handleSuccessfulLogin(userData);
            // Continue with your login logic...
        } catch (error) {
            console.error('Error during login:', error);
        }
    };

    const fetchUserData = async () => {
        try {
            const response = await fetch('api/login/SignIn', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });

            const data = await response.json();

            if (response.ok) {
                // Registration successful
                if (data.status === 1) {
                   
                    setApiStatus('User Logged in successfully!');
                    return data.result; // Return user data
                } else {
                    setApiStatus('Error: ' + data.message);
                    console.error('Error:', data.message);
                    throw new Error(data.message);
                }
            } else {
                // Registration failed
                setApiStatus('Login failed!');
                console.error('Login failed');
                throw new Error('Login failed');
            }
        } catch (error) {
            setApiStatus('Error during Login');
            console.error('Error during Login:', error);
            throw error;
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            await handleSuccessfulLoginLocal();
        } catch (error)
        {
            // Handle errors if needed
        }
    };

    return (
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-1">
                        <div className="heading_container">
                            <h2>Login</h2>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-6 col-md-11 offset-md-1">
                        <div className="form_container">
                            <form onSubmit={handleSubmit}>
                                <div>
                                    <input
                                        name="EmailId"
                                        value={formData.EmailId}
                                        onChange={handleChange}
                                        type="email"
                                        placeholder="Email Id"
                                    />
                                </div>
                                <div>
                                    <input type="text" placeholder="OR" disabled />
                                </div>
                                <div>
                                    <input
                                        name="PhoneNo"
                                        value={formData.PhoneNo}
                                        onChange={handleChange}
                                        type="text"
                                        placeholder="Phone Number"
                                    />
                                </div>
                                <div>
                                    <input
                                        name="Password"
                                        value={formData.Password}
                                        onChange={handleChange}
                                        type="password"
                                        placeholder="password"
                                    />
                                </div>

                                <div className="btn_box">
                                    <button type="submit">Login</button>
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
    );
};

export default Login;
