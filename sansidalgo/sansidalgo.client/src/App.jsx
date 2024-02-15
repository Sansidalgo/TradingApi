import React from 'react';
import { NavLink, Outlet, useNavigate} from 'react-router-dom';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Button from '@mui/material/Button';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBell } from '@fortawesome/free-solid-svg-icons';
import { faAngleDown } from '@fortawesome/free-solid-svg-icons';
import Notifications from './Notifications'

const App = ({ user, isLoggedIn,notificationCountInApp }) => {

    const [anchorAnalyze, setAnchorAnalyze] = React.useState(null);
    const openAnalyze = Boolean(anchorAnalyze);
    const handleClickAnalyze = (event) => {
        setAnchorAnalyze(event.currentTarget);
    };
    const handleCloseAnalyze = () => {
        setAnchorAnalyze(null);
    };
    const [anchorTrade, setAnchorTrade] = React.useState(null);
    const openTrade = Boolean(anchorTrade);
    const handleClickTrade = (event) => {
        setAnchorTrade(event.currentTarget);
    };
    const handleCloseTrade = () => {
        setAnchorTrade(null);
    };
    const [anchorEl, setAnchorEl] = React.useState(null);
    const open = Boolean(anchorEl);
    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    const navigate = useNavigate();

    const handleMenuButtonClick = (url) => {
        navigate(`/${url}`);
    };
    console.log(notificationCountInApp)
    console.log("check check")
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
                                            <Button className="demo-positioned-button" onClick={() => handleMenuButtonClick("home")}>
                                                Home
                                            </Button>
                                            
                                            
                                        </li>
                                        <li className="nav-item">
                                            <Button className="demo-positioned-button" onClick={()=>handleMenuButtonClick("chatx")}>
                                                ChatX
                                            </Button>
                                            
                                        </li>
                                        {isLoggedIn && (
                                            <> 
                                                <li className="nav-item">
                                                    <Button
                                                        className="demo-positioned-button"
                                                        aria-controls={openTrade ? 'demo-positioned-menu' : undefined}
                                                        aria-haspopup="true"
                                                        aria-expanded={openTrade ? 'true' : 'false'}
                                                        onClick={handleClickTrade}
                                                    >
                                                        Trade
                                                        <FontAwesomeIcon icon={faAngleDown} className="angle-down-icon" />
                                                    </Button></li>
                                                    <li className="nav-item">
                                                    <Menu
                                                        anchorEl={anchorTrade}
                                                        open={Boolean(anchorTrade)}
                                                        onClose={handleCloseTrade}
                                                        sx={{ paddingX: '16px' }}                                                    >
                                                        <MenuItem onClick={handleCloseTrade}>
                                                            <NavLink className="nav-link" activeclassname="active" to="/Portfolio">
                                                                Portfolio
                                                            </NavLink>
                                                        </MenuItem>
                                                        <MenuItem onClick={handleCloseTrade}>
                                                            <NavLink className="nav-link" activeclassname="active" to="/Delegate">
                                                                Delegate
                                                            </NavLink>
                                                        </MenuItem>
                                                        <MenuItem onClick={handleCloseTrade}>
                                                            <NavLink className="nav-link" to="/Pnl">
                                                                P&L
                                                            </NavLink>
                                                        </MenuItem>
                                                    </Menu>


                                                </li>
                                            </>
                                        )}
                                        <li className="nav-item">
                                            <Button
                                                className="demo-positioned-button"
                                                aria-controls={openAnalyze ? 'demo-positioned-menu' : undefined}
                                                aria-haspopup="true"
                                                aria-expanded={openAnalyze ? 'true' : 'false'}
                                                onClick={handleClickAnalyze}
                                            >
                                                Analyse
                                                <FontAwesomeIcon icon={faAngleDown} className="angle-down-icon" />
                                            </Button>
                                            </li>
                                            <li className="nav-item">
                                            <Menu
                                                anchorEl={anchorAnalyze}
                                                open={Boolean(anchorAnalyze)}
                                                onClose={handleCloseAnalyze}
                                                sx={{ paddingX: '16px' }} >
                                                <MenuItem onClick={handleCloseAnalyze}>
                                                    <NavLink className="nav-link" activeclassname="active" to="/oi">
                                                        Option Chain
                                                    </NavLink>
                                                </MenuItem>
                                                <MenuItem onClick={handleCloseAnalyze}>
                                                    <NavLink className="nav-link" to="/tradingviewchart">
                                                        Trading View
                                                    </NavLink>
                                                </MenuItem>
                                                <MenuItem onClick={handleCloseAnalyze}>
                                                    <NavLink className="nav-link" to="/TopMovers">
                                                        Top Movers
                                                    </NavLink>
                                                </MenuItem>
                                            </Menu>

                                            
                                        </li>
                                       

                                       
                                        {isLoggedIn ? (
                                            <> 
                                            <li className="nav-item">
                                                <Button
                                                    className="demo-positioned-button"
                                                    aria-controls={open ? 'demo-positioned-menu' : undefined}
                                                    aria-haspopup="true"
                                                    aria-expanded={open ? 'true' : undefined}
                                                    onClick={handleClick}
                                                >
                                                    <span>{user.name}</span>
                                                    <FontAwesomeIcon icon={faAngleDown} className="angle-down-icon"  />
                                                </Button>
                                                </li>
                                            <li className="nav-item">
                                                    <Menu
                                                    anchorEl={anchorEl}
                                                    open={Boolean(anchorEl)}
                                                        onClose={handleClose}
                                                        sx={{ paddingX: '16px' }}
                                                        PaperProps={{
                                                            sx: {
                                                                width: '245px', // Set the desired width
                                                                paddingX: '16px',
                                                            },
                                                        }}
                                                    >
                                                        <MenuItem onClick={handleClose}>
                                                            <NavLink className="nav-link" activeclassname="active" to="/ordersettings">
                                                                Order Settings
                                                            </NavLink>
                                                        </MenuItem>
                                                        <MenuItem onClick={handleClose}>
                                                            <NavLink className="nav-link" to="/subscriptions">
                                                                Subscriptions
                                                            </NavLink>
                                                        </MenuItem>
                                                        <MenuItem onClick={handleClose}>
                                                            <NavLink className="nav-link" to="/userguides">
                                                                User Guides
                                                            </NavLink>
                                                        </MenuItem>
                                                        <MenuItem onClick={handleClose}>
                                                            <NavLink className="nav-link" to="/logout">
                                                                Logout
                                                            </NavLink>
                                                        </MenuItem>
                                                    </Menu>
                                                </li>
                                            </>    
                                        ) : (
                                            <>
                                                <li className="nav-item">
                                                <NavLink className="nav-link" activeclassname="active" to="/login">
                                                    <i className="fa fa-user" aria-hidden="true"></i>Login
                                                </NavLink>
                                                </li>
                                            </>
                                            )}
                                        
                                        {isLoggedIn && (
                                            <>

                                                <li className="nav-item">
                                                    <NavLink to="/Notifications" activeclassname="active-link" className="notification-link">
                                                        {notificationCountInApp > 0 && <span className="notification-badge">{notificationCountInApp}</span>}
                                                        <FontAwesomeIcon icon={faBell} size="2x" className="notification-bell" />
                                                    </NavLink>
                                                </li>
                                            </>
                                        )}
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


            {/*<!-- service section -->*/}

            <section className="service_section layout_padding">
                <div className="service_container">
                    <div className="container ">
                        <div className="heading_container">
                            <h2>
                                Our <span>Services</span>
                            </h2>
                            <p>
                                We offer an extensive array of software solutions, AI/ML capabilities, and automated services.
                            </p>
                        </div>
                        <div className="row">
                            <div className="col-md-6 ">
                                <div className="box ">
                                    <div className="img-box">
                                        <img src="images/s11.png" alt=""/>
                                    </div>
                                    <div className="detail-box">
                                        <h5>
                                            Automated Trading
                                        </h5>
                                        <p>
                                            Embrace the future of trading with Trade Synergies. Our advanced technology empowers you to make informed decisions and stay ahead in the market.
                                        </p>
                                        <a href="#">
                                            Read More
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-6 ">
                                <div className="box ">
                                    <div className="img-box">
                                        <img src="images/s22.png" alt=""/>
                                    </div>
                                    <div className="detail-box">
                                        <h5>
                                            Master Copy Trading
                                        </h5>
                                        <p>
                                            Execute trades seamlessly across various brokerage accounts using a unified system, allowing for copying trades and performing bulk modifications, cancellations, and square-offs
                                        </p>
                                        <a href="#">
                                            Read More
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-6 ">
                                <div className="box ">
                                    <div className="img-box">
                                        <img src="images/s33.png" alt=""/>
                                    </div>
                                    <div className="detail-box">
                                        <h5>
                                            Introducing ChatX
                                        </h5>
                                        <p>
                                            A powerful conversational AI platform designed to enhance communication. Similar to ChatGPT, ChatX employs advanced language models to provide intelligent and natural interactions, making it an ideal solution for various applications such as customer support, virtual assistants, and more.
                                        </p>
                                        <a href="#">
                                            Read More
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-6 ">
                                <div className="box ">
                                    <div className="img-box">
                                        <img src="images/s44.png" alt=""/>
                                    </div>
                                    <div className="detail-box">
                                        <h5>
                                            Software Services
                                        </h5>
                                        <p>
                                            Our software services company is dedicated to delivering cutting-edge solutions tailored to meet your business needs. With a focus on innovation and expertise, we provide a comprehensive range of services, from custom software development to system integration, ensuring optimal efficiency and success for our clients in the ever-evolving digital landscape.
                                        </p>
                                        <a href="#">
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
                        <span> All Rights Reserved By <a href="https://sansidalgo.com">Sansid Technologies</a> ||
                          
                            Distributed By <a href="https://sansidalgo.com">Sansid Technologies</a> ||

                            
                            Charts are powered by <a href="https://www.tradingview.com/">TradingView</a>

                        </span>
                    </p>
                </div>
            </section>
        </div>
    );
};

export default App;
