import React from 'react';
import { NavLink, Outlet } from 'react-router-dom';

const App = ({ user, isLoggedIn }) => {
    return (
        <div>
            <div className="hero_area">
                <header className="header_section">
                    <div className="header_top">
                        <div className="container-fluid ">
                            <div className="contact_nav">
                                <a href="">
                                    <i className="fa fa-phone" aria-hidden="true"></i>
                                    <span>Call : +91 8555850061</span>
                                </a>
                                <a href="">
                                    <i className="fa fa-envelope" aria-hidden="true"></i>
                                    <span>Email : admin@sansidalgo.com</span>
                                </a>
                                <a href="">
                                    <i className="fa fa-map-marker" aria-hidden="true"></i>
                                    <span>Location</span>
                                </a>
                            </div>
                        </div>
                    </div>
                    <div className="header_bottom">
                        <div className="container-fluid">
                            <nav className="navbar navbar-expand-lg custom_nav-container">
                                <NavLink className="navbar-brand" to="/home">
                                    <span>Trade Synergies</span>
                                </NavLink>
                                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                    <span className=""> </span>
                                </button>
                                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                                    <ul className="navbar-nav">
                                        <li className="nav-item">
                                            <NavLink className="nav-link" activeclassname="active" to="/home">
                                                Home
                                            </NavLink>
                                        </li>
                                        <li className="nav-item">
                                            <NavLink className="nav-link" activeclassname="active" to="/userguides">
                                                User Guides
                                            </NavLink>
                                        </li>
                                        {isLoggedIn && (
                                            <>
                                                <li className="nav-item">
                                                    <NavLink className="nav-link" activeclassname="active" to="/portfolio">
                                                        Portfolio
                                                    </NavLink>
                                                </li>
                                            </>
                                        )}

                                        {isLoggedIn && (
                                            <>
                                                <li className="nav-item">
                                                    <NavLink className="nav-link" activeclassname="active" to="/ordersettings">
                                                        Order Settings
                                                    </NavLink>
                                                </li>
                                            </>
                                        )}

                                        <li className="nav-item">
                                            {isLoggedIn ? (
                                                <NavLink className="nav-link" to="/logout">
                                                    <span>{user.name}</span>
                                                    <i className="fa fa-user" aria-hidden="true"></i>Logout
                                                </NavLink>
                                            ) : (
                                                    <NavLink className="nav-link" activeclassname="active" to="/login">
                                                    <i className="fa fa-user" aria-hidden="true"></i>Login
                                                </NavLink>
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
            </div>

            <section className="info_section layout_padding2">
                <div className="container">
                    <div className="row">
                        <div className="col-md-6 col-lg-3 info_col">
                            <div className="info_contact">
                                <h4>Address</h4>
                                <div className="contact_link_box">
                                    <a href="">
                                        <i className="fa fa-map-marker" aria-hidden="true"></i>
                                        <span>Location</span>
                                    </a>
                                    <a href="">
                                        <i className="fa fa-phone" aria-hidden="true"></i>
                                        <span>Call +91 8555850061</span>
                                    </a>
                                    <a href="">
                                        <i className="fa fa-envelope" aria-hidden="true"></i>
                                        <span>admin@sansidalgo.com</span>
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
                                <h4>Info</h4>
                                <p>necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful</p>
                            </div>
                        </div>
                        <div className="col-md-6 col-lg-2 mx-auto info_col">
                            <div className="info_link_box">
                                <h4>Links</h4>
                                <div className="info_links">
                                    <NavLink className="active" to="/home">Home</NavLink>
                                    <NavLink className="active" to="/home">Contact</NavLink>
                                    <NavLink className="active" to="/home">Services</NavLink>
                                    <NavLink className="active" to="/home">Clients</NavLink>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-6 col-lg-3 info_col ">
                            <h4>Subscribe</h4>
                            <form action="#">
                                <input type="text" placeholder="Enter email" />
                                <button type="submit">Subscribe</button>
                            </form>
                        </div>
                    </div>
                </div>
            </section>

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
};

export default App;
