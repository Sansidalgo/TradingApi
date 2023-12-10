import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter as Router, Routes, Route, Link, Outlet } from 'react-router-dom';

import { useEffect, useState } from 'react';
import Slider from './slider.jsx'
import SignUp from './signup.jsx'
import Login from './login.jsx'
import Logout from './logout.jsx'

const App = ({ user, isLoggedIn }) => {

    

    return (

        <div >

            <div className="hero_area">
                {/*     <!-- header section strats -->*/}
                <header className="header_section">
                    <div className="header_top">
                        <div className="container-fluid ">
                            <div className="contact_nav">
                                <a href="">
                                    <i className="fa fa-phone" aria-hidden="true"></i>
                                    <span>
                                        Call : +91 8555850061
                                    </span>
                                </a>
                                <a href="">
                                    <i className="fa fa-envelope" aria-hidden="true"></i>
                                    <span>
                                        Email : admin@sansidalgo.com
                                    </span>
                                </a>
                                <a href="">
                                    <i className="fa fa-map-marker" aria-hidden="true"></i>
                                    <span>
                                        Location
                                    </span>
                                </a>
                            </div>
                        </div>
                    </div>
                    <div className="header_bottom">
                        <div className="container-fluid">
                            <nav className="navbar navbar-expand-lg custom_nav-container ">
                               
                                <Link className="navbar-brand" to="/">  <span>
                                    Trade Synergies
                                </span></Link>

                            



                                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                    <span className=""> </span>
                                </button>

                                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                                    <ul className="navbar-nav  ">
                                        <li className="nav-item active">
                                            <Link className="nav-link" to="/">Home<span className="sr-only">(current)</span></Link>
                                                                                  
                                        </li>
                                        <li className="nav-item">
                                            <Link className="nav-link" to="/stockbrokers">Stock Brokers</Link>

                                        </li>
                                       
                                        <li className="nav-item">
                                            {isLoggedIn ? (
                                                <Link className="nav-link" to="/logout"><span>{user.name}</span><i className="fa fa-user" aria-hidden="true"></i>Logout</Link>


                                            ) : (
                                                <Link className="nav-link" to="/login"><i className="fa fa-user" aria-hidden="true"></i>Login</Link>

                                            )}



                                        </li>

                                        <form className="form-inline">
                                            <button className="btn  my-2 my-sm-0 nav_search-btn" type="submit">
                                                <i className="fa fa-search" aria-hidden="true"></i>
                                            </button>
                                        </form>
                                    </ul>
                                </div>
                            </nav>
                        </div>
                    </div>
                </header>
                {/*<!-- pages will be loaded here -->*/}
                <Outlet />
                {/*<!--end pages will be loaded here -->*/}
            </div>



            {/*<!-- info section -->*/}

            <section className="info_section layout_padding2">
                <div className="container">
                    <div className="row">
                        <div className="col-md-6 col-lg-3 info_col">
                            <div className="info_contact">
                                <h4>
                                    Address
                                </h4>
                                <div className="contact_link_box">
                                    <a href="">
                                        <i className="fa fa-map-marker" aria-hidden="true"></i>
                                        <span>
                                            Location
                                        </span>
                                    </a>
                                    <a href="">
                                        <i className="fa fa-phone" aria-hidden="true"></i>
                                        <span>
                                            Call +91 8555850061
                                        </span>
                                    </a>
                                    <a href="">
                                        <i className="fa fa-envelope" aria-hidden="true"></i>
                                        <span>
                                            admin@sansidalgo.com
                                        </span>
                                    </a>
                                </div>
                            </div>
                            <div className="info_social">
                                <a href="">
                                    <i className="fa fa-facebook" aria-hidden="true"></i>
                                </a>
                                <a href="">
                                    <i className="fa fa-twitter" aria-hidden="true"></i>
                                </a>
                                <a href="">
                                    <i className="fa fa-linkedin" aria-hidden="true"></i>
                                </a>
                                <a href="">
                                    <i className="fa fa-instagram" aria-hidden="true"></i>
                                </a>
                            </div>
                        </div>
                        <div className="col-md-6 col-lg-3 info_col">
                            <div className="info_detail">
                                <h4>
                                    Info
                                </h4>
                                <p>
                                    necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful
                                </p>
                            </div>
                        </div>
                        <div className="col-md-6 col-lg-2 mx-auto info_col">
                            <div className="info_link_box">
                                <h4>
                                    Links
                                </h4>
                                <div className="info_links">
                                    <a className="active" href="index.html">
                                        <img src="images/nav-bullet.png" alt="" />
                                        Home
                                    </a>
                                    <a className="" href="about.html">
                                        <img src="images/nav-bullet.png" alt="" />
                                        About
                                    </a>
                                    <a className="" href="service.html">
                                        <img src="images/nav-bullet.png" alt="" />
                                        Services
                                    </a>
                                    <a className="" href="contact.html">
                                        <img src="images/nav-bullet.png" alt="" />
                                        Contact Us
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-6 col-lg-3 info_col ">
                            <h4>
                                Subscribe
                            </h4>
                            <form action="#">
                                <input type="text" placeholder="Enter email" />
                                <button type="submit">
                                    Subscribe
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </section>

            {/*<!-- end info section -->*/}

            {/*<!-- footer section -->*/}
            <section className="footer_section">
                <div className="container">
                    <p>
                        &copy; <span id="displayYear"></span> All Rights Reserved By
                        <a href="https://sansidalgo.com">Sansid Technologies</a>
                        Distributed By
                        <a href="https://sansidalgo.com">Sansid Technologies</a>
                    </p>
                </div>
            </section>

        </div>
    );

}

export default App;