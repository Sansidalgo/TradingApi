import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Routes, Route, Link, Outlet, Navigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import App from './App.jsx'
import Home from './slider.jsx'
import Login from './login.jsx'
import Signup from './signup.jsx'
import Logout from './logout.jsx'
const Main = () => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    useEffect(() => {
        // Check for the presence of the authentication token in localStorage
        const token = localStorage.getItem('token');
        if (token) {
            // If the token is present, set isLoggedIn to true
            setIsLoggedIn(true);
        }
    }, []); // Empty dependency array ensures this effect runs only once on mount

    // Function to handle login and logout
    const handleAuthAction = (isLogin) => {
        if (isLogin) {
            // Perform login logic here (e.g., make API request, set authentication token)
            setIsLoggedIn(true);
        } else {
            // Perform logout logic here (e.g., clear authentication token)
            setIsLoggedIn(false);
        }
    };
    return (<React.StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<App isLoggedIn={isLoggedIn} />} >
                    <Route index   element={<Home />} />
                    <Route
                        path="Login"
                        element={
                            isLoggedIn ? (
                                <Navigate to="/" />
                            ) : (
                                <Login handleSuccessfulLogin={() => handleAuthAction(true)} />
                            )
                        }
                    />

                    <Route path="signup" element={<Signup />} />
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
    </React.StrictMode>);


};
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<Main />);