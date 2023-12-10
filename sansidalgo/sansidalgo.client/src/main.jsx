import React from 'react'
import ReactDOM from 'react-dom/client'
import * as ReactDOMClient from 'react-dom/client';
import { BrowserRouter, Routes, Route, Link, Outlet, Navigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { checkTokenExpiration } from './authHelpers';

import { jwtDecode } from 'jwt-decode';
import App from './App.jsx'
import Home from './slider.jsx'
import Login from './login.jsx'
import Signup from './signup.jsx'
import Logout from './logout.jsx'
import Stockbrokers from './stockbrokers.jsx'
import Shoonya from './Brokers/shoonya.jsx'

const Main = () => {
    const initialUserData = {
        Id: '',
        Name:'',
        EmailId: '',
        PhoneNo: '',
        Password: '',
        Role: '',
        Token:''
    };
    const [user, setUser] = useState(initialUserData);
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        // Check for the presence of the authentication token in localStorage
        const { status, user } = checkTokenExpiration();
       
        console.log('Is token valid?', status);
        if (status) {
            // If the token is present, set isLoggedIn to true
            setUser(user);
            setIsLoggedIn(true);
            // Fetch user data based on the token and update the user state
          
        }
    }, []); // Empty dependency array ensures this effect runs only once on mount


   
    // Function to handle login and logout
    const handleAuthAction = (isLogin, userData) => {
        if (isLogin) {
            // Perform login logic here (e.g., set authentication token)
            setIsLoggedIn(true);
            // Update the user information in the state
            setUser(userData);
        } else {
            // Perform logout logic here (e.g., clear authentication token)
            setIsLoggedIn(false);
            // Clear the user information in the state
            setUser(null);
        }
    };

   
    return (
        <React.StrictMode>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<App user={user} isLoggedIn={isLoggedIn} />} >
                        <Route index element={<Home />} />
                        <Route
                            path="Login"
                            element={
                                isLoggedIn ? (
                                    <Navigate to="/" />
                                ) : (
                                    <Login
                                        handleSuccessfulLogin={(userData) =>
                                            handleAuthAction(true, userData)
                                        }
                                    />
                                )
                            }
                        />

                        <Route path="signup" element={<Signup />} />
                        <Route path="stockbrokers" element={<Stockbrokers />} />
                        <Route path="shoonya" element={<Shoonya/>} />
                        
                        <Route
                            path="logout"
                            element={
                                isLoggedIn ? (
                                    <Logout handleLogout={() => handleAuthAction(false)} />
                                ) : (
                                    <Navigate to="/" />
                                )
                            }
                        />
                    </Route>

                    
                </Routes>
            </BrowserRouter>
        </React.StrictMode>
    );
};

// Create a root only once
const container = document.getElementById('root');
const root = ReactDOMClient.createRoot(container);

// Use root.render() to update the component tree
root.render(<Main />);