import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter as Router, Routes, Route, Link, Outlet } from 'react-router-dom';

import { useEffect, useState } from 'react';
import Slider from './slider.jsx'
import SignUp from './signup.jsx'
import Login from './login.jsx'
import Logout from './logout.jsx'

const App = ({ isLoggedIn }) => {

    

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
                                <a className="navbar-brand" href="index.html">
                                    <span>
                                        Trade Synergies
                                    </span>
                                </a>

                                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                    <span className=""> </span>
                                </button>

                                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                                    <ul className="navbar-nav  ">
                                        <li className="nav-item active">
                                            <Link className="nav-link" to="/">Home<span className="sr-only">(current)</span></Link>
                                                                                  
                                        </li>
                                        <li className="nav-item">
                                            <a className="nav-link" href="service.html">Services</a>
                                        </li>
                                        <li className="nav-item">
                                            <a className="nav-link" href="about.html"> About</a>
                                        </li>
                                        <li className="nav-item">
                                            <a className="nav-link" href="contact.html">Contact Us</a>
                                        </li>
                                        <li className="nav-item">
                                            {isLoggedIn ? (
                                                <Link className="nav-link" to="/logout"><i className="fa fa-user" aria-hidden="true"></i>Logout</Link>


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
                <Outlet />
                {/*<!-- end header section -->*/}
                {/*<!-- slider section -->*/}

                {/*<Router>*/}
                {/*    <Routes>*/}
                {/*        <Route path="/" element={<Slider />} />*/}

                {/*        <Route path="/login" element={<Login handleSuccessfulLogin={() => handleAuthAction(true)} />} />*/}
                {/*        <Route path="/logout" element={<Logout handleLogout={() => handleAuthAction(false)} />} />*/}

                {/*        */}{/* Add more route definitions here... */}
                {/*    </Routes>*/}
                {/*</Router>*/}

                {/*<RouterProvider*/}
                {/*    router={router}*/}
                {/*    fallbackElement={<Slider />}*/}
                {/*/>*/}

                {/*     <!-- end slider section -->*/}
            </div>


            {/*    <!-- service section -->*/}

            <section className="service_section layout_padding">
                <div className="service_container">
                    <div className="container ">
                        <div className="heading_container">
                            <h2>
                                Our <span>Services</span>
                            </h2>
                            <p>
                                There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration
                            </p>
                        </div>
                        <div className="row">
                            <div className="col-md-6 ">
                                <div className="box ">
                                    <div className="img-box">
                                        <img src="images/s1.png" alt="" />
                                    </div>
                                    <div className="detail-box">
                                        <h5>
                                            Air Transport
                                        </h5>
                                        <p>
                                            fact that a reader will be distracted by the readable content of a page when looking at its layout.
                                            The
                                            point of using
                                        </p>
                                        <a href="">
                                            Read More
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-6 ">
                                <div className="box ">
                                    <div className="img-box">
                                        <img src="images/s2.png" alt="" />
                                    </div>
                                    <div className="detail-box">
                                        <h5>
                                            Cargo Transport
                                        </h5>
                                        <p>
                                            fact that a reader will be distracted by the readable content of a page when looking at its layout.
                                            The
                                            point of using
                                        </p>
                                        <a href="">
                                            Read More
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-6 ">
                                <div className="box ">
                                    <div className="img-box">
                                        <img src="images/s3.png" alt="" />
                                    </div>
                                    <div className="detail-box">
                                        <h5>
                                            Trucks Transport
                                        </h5>
                                        <p>
                                            fact that a reader will be distracted by the readable content of a page when looking at its layout.
                                            The
                                            point of using
                                        </p>
                                        <a href="">
                                            Read More
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-6 ">
                                <div className="box ">
                                    <div className="img-box">
                                        <img src="images/s4.png" alt="" />
                                    </div>
                                    <div className="detail-box">
                                        <h5>
                                            Train Transport
                                        </h5>
                                        <p>
                                            fact that a reader will be distracted by the readable content of a page when looking at its layout.
                                            The
                                            point of using
                                        </p>
                                        <a href="">
                                            Read More
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/*<!-- end service section -->*/}


            {/*<!-- about section -->*/}

            <section className="about_section layout_padding-bottom">
                <div className="container  ">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="detail-box">
                                <div className="heading_container">
                                    <h2>
                                        About <span>Us</span>
                                    </h2>
                                </div>
                                <p>
                                    There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration
                                    in some form, by injected humour, or randomised words which don't look even slightly believable. If you
                                    are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in
                                    the middle of text. All
                                </p>
                                <a href="">
                                    Read More
                                </a>
                            </div>
                        </div>
                        <div className="col-md-6 ">
                            <div className="img-box">
                                <img src="images/about-img.jpg" alt="" />
                            </div>
                        </div>

                    </div>
                </div>
            </section>

            {/*<!-- end about section -->*/}

            {/*<!-- track section -->*/}

            <section className="track_section layout_padding">
                <div className="track_bg_box">
                    <img src="images/track-bg.jpg" alt="" />
                </div>
                <div className="container">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="heading_container">
                                <h2>
                                    Track Your Shipment
                                </h2>
                            </div>
                            <p>
                                Iste reprehenderit maiores facilis saepe cumque molestias. Labore iusto excepturi, laborum aliquid pariatur veritatis autem, mollitia sint nesciunt hic error porro.
                                Deserunt officia unde repellat beatae ipsum sed. Aperiam tempora consectetur voluptas magnam maxime asperiores quas similique repudiandae, veritatis reiciendis harum fuga atque.
                            </p>
                            <form action="">
                                <input type="text" placeholder="Enter Your Tracking Number" />
                                <button type="submit">
                                    Track
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </section>

            {/*<!-- end track section -->*/}

            {/*<!-- client section -->*/}

            <section className="client_section layout_padding">
                <div className="container">
                    <div className="heading_container">
                        <h2>
                            What Says Our <span>Client</span>
                        </h2>
                    </div>
                    <div className="client_container">
                        <div className="carousel-wrap ">
                            <div className="owl-carousel">
                                <div className="item">
                                    <div className="box">
                                        <div className="detail-box">
                                            <p>
                                                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
                                            </p>
                                        </div>
                                        <div className="client_id">
                                            <div className="img-box">
                                                <img src="images/client-1.png" alt="" className="img-1" />
                                            </div>
                                            <div className="name">
                                                <h6>
                                                    Adipiscing
                                                </h6>
                                                <p>
                                                    Magna
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="item">
                                    <div className="box">
                                        <div className="detail-box">
                                            <p>
                                                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
                                            </p>
                                        </div>
                                        <div className="client_id">
                                            <div className="img-box">
                                                <img src="images/client-2.png" alt="" className="img-1" />
                                            </div>
                                            <div className="name">
                                                <h6>
                                                    Adipiscing
                                                </h6>
                                                <p>
                                                    Magna
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="item">
                                    <div className="box">
                                        <div className="detail-box">
                                            <p>
                                                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
                                            </p>
                                        </div>
                                        <div className="client_id">
                                            <div className="img-box">
                                                <img src="images/client-1.png" alt="" className="img-1" />
                                            </div>
                                            <div className="name">
                                                <h6>
                                                    Adipiscing
                                                </h6>
                                                <p>
                                                    Magna
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="item">
                                    <div className="box">
                                        <div className="detail-box">
                                            <p>
                                                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
                                            </p>
                                        </div>
                                        <div className="client_id">
                                            <div className="img-box">
                                                <img src="images/client-2.png" alt="" className="img-1" />
                                            </div>
                                            <div className="name">
                                                <h6>
                                                    Adipiscing
                                                </h6>
                                                <p>
                                                    Magna
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/*<!-- end client section -->*/}

            {/*<!-- contact section -->*/}
            <section className="contact_section">
                <div className="container-fluid">
                    <div className="row">
                        <div className="col-lg-4 col-md-5 offset-md-1">
                            <div className="heading_container">
                                <h2>
                                    Contact Us
                                </h2>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-lg-4 col-md-5 offset-md-1">
                            <div className="form_container contact-form">
                                <form action="">
                                    <div>
                                        <input type="text" placeholder="Your Name" />
                                    </div>
                                    <div>
                                        <input type="text" placeholder="Phone Number" />
                                    </div>
                                    <div>
                                        <input type="email" placeholder="Email" />
                                    </div>
                                    <div>
                                        <input type="text" className="message-box" placeholder="Message" />
                                    </div>
                                    <div className="btn_box">
                                        <button>
                                            SEND
                                        </button>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <div className="col-lg-7 col-md-6 px-0">
                            <div className="map_container">
                                <div className="map">
                                    <div id="googleMap"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            {/*<!-- end contact section -->*/}

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