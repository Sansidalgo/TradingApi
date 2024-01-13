import React from 'react'

import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { checkTokenExpiration } from './authHelpers';
import App from './App.jsx'
import Home from './Home'
import Login from './Login'
import Signup from './Signup'
import Logout from './Logout'
import Stockbrokers from './StockBrokers'
import OrderSettings from './OrderSettings'
import OrderSetting from './OrderSetting'
import Portfolio from './Portfolio'
import Plans from './Plans'
import ChatGPTWithGoogleGemini from './ChatGPTWithGoogleGemini'
import UserGuides from './UserGuides';
import Subscriptions from './Subscriptions/Subscriptions';
import TradingViewChart from './TradingView/TradingViewChart'
const Router = () => {
    const initialUserData = {
        Id: '',
        Name: '',
        EmailId: '',
        PhoneNo: '',
        Password: '',
        Role: '',
        Token: ''
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
            localStorage.removeItem('userData')
            // Clear the user information in the state
            setUser(null);
        }
    };


    return (

        <BrowserRouter>
            <Routes>
                <Route path="/" element={<App user={user} isLoggedIn={isLoggedIn} />} >
                    {/* <Route index element={<Home />} />*/}
                    <Route
                        index
                        element={
                            isLoggedIn ? (
                                <Navigate to="/portfolio" />
                            ) : (
                                <Home />
                            )
                        }
                    />
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
                    <Route
                        path="subscriptions"
                        element={
                            isLoggedIn ? (
                                <Subscriptions />
                            ) : (
                                <Login
                                    handleSuccessfulLogin={(userData) =>
                                        handleAuthAction(true, userData)
                                    }
                                />
                            )
                        }
                    />

                    <Route
                        path="ordersettings"
                        element={
                            isLoggedIn ? (
                                <OrderSettings />
                            ) : (
                                <Login
                                    handleSuccessfulLogin={(userData) =>
                                        handleAuthAction(true, userData)
                                    }
                                />
                            )
                        }
                    />
                    <Route
                        path="ordersetting/:orderSettingId"
                        element={
                            isLoggedIn ? (
                                <OrderSetting />
                            ) : (
                                <Login
                                    handleSuccessfulLogin={(userData) =>
                                        handleAuthAction(true, userData)
                                    }
                                />
                            )
                        }
                    />
                    <Route
                        path="portfolio"
                        element={
                            isLoggedIn ? (
                                <Portfolio />
                            ) : (
                                <Login
                                    handleSuccessfulLogin={(userData) =>
                                        handleAuthAction(true, userData)
                                    }
                                />
                            )
                        }
                    />
                    <Route
                        path="plans"
                        element={
                            isLoggedIn ? (
                                <Plans />
                            ) : (
                                <Login
                                    handleSuccessfulLogin={(userData) =>
                                        handleAuthAction(true, userData)
                                    }
                                />
                            )
                        }
                    />

                    <Route path="home" element={<Home />} />
                    <Route path="tradingviewchart" element={<TradingViewChart />} />
                    <Route path="chatx" element={<ChatGPTWithGoogleGemini />} />
                    <Route path="signup" element={<Signup />} />

                    <Route path="stockbrokers" element={<Stockbrokers />} />
                    <Route path="userguides" element={<UserGuides />} />
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

    );
};
export default Router;
