import React from 'react'
import ReactDOM from 'react-dom/client'
import { useEffect, useState } from 'react';

function signup() {

    const [formData, setFormData] = useState({
        Name: '',
        EmailId: '',
        PhoneNo: '',
        Password: '',
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('api/Login/SignUp', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });

            if (response.ok) {
                // Registration successful
                console.log('User registered successfully!');
            } else {
                // Registration failed
                console.error('Registration failed');
            }
        } catch (error) {
            console.error('Error during registration:', error);
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
                                Sign Up
                            </h2>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-6 col-md-11 offset-md-1">
                        <div className="form_container">
                            <form onSubmit={handleSubmit} type="submit">
                                <div>
                                    <input name="Name" value={formData.Name} onChange={handleChange} type="text" placeholder="name"  />
                                </div>
                                <div>
                                    <input name="EmailId" value={formData.EmailId} onChange={handleChange} type="email" placeholder="Email Id" />
                                </div>
                                <div>
                                    <input name="PhoneNo" value={formData.PhoneNo} onChange={handleChange} type="text" placeholder="Phone Number" />
                                </div>
                                <div>
                                    <input name="Password" value={formData.Password} onChange={handleChange} type="password" placeholder="password" />
                                </div>

                                <div className="btn_box">
                                    <button>
                                        Sign Up
                                    </button>
                                </div>
                                <div className="second-div">

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
export default signup;