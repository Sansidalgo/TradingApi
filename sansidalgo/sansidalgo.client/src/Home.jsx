import React from 'react'
import { Link } from 'react-router-dom';

const Home = ({ isLoggedIn }) => {
    console.log(isLoggedIn)
    console.log("check login")
    return (
        <section className="slider_section">
            <div className="slider_bg_box">
                <img src="images/slider-bg.jpg" alt="" />
            </div>
            <div id="customCarousel1" className="carousel slide" data-ride="carousel">
                <div className="carousel-inner">
                    <div className="carousel-item active">
                        <div className="container">
                            <div className="row">
                                <div className="col-md-7">
                                    <div className="detail-box">
                                        <div>
                                            <h1>
                                                Elevate Your Trading with Our Premier Automated Services
                                            </h1>

                                            

                                            {!isLoggedIn ? (
                                                <div>
                                                    <p>
                                                        Ready to enhance your trading experience? Join us today and embark on a journey towards automated trading success with Trade Synergies.
                                                    </p>

                                                    <div className="btn-box">
                                                        <Link className="btn1" to="/signup">Sign Up<span className="sr-only">(current)</span></Link>
                                                    </div>
                                                </div>
                                            ) : (
                                            <div>
                                                <p>
                                                    Experience the pinnacle of automated trading with Trade Synergies. Our comprehensive plans are designed to revolutionize your trading journey.
                                                </p>

                                                <div className="btn-box">
                                                            <Link className="btn1" to="/subscriptions">Explore Plans<span className="sr-only">(current)</span></Link>
                                                </div>
                                            </div>                                            
                                            )}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="carousel-item">
                        <div className="container">
                            <div className="row">
                                <div className="col-md-7">
                                    <div className="detail-box">
                                        <div>
                                            <h1>
                                                Unlock Your Trading Potential with Cutting-Edge Technology
                                            </h1>

                                            

                                            {!isLoggedIn ? (
                                                <div>
                                                    <p>
                                                        Ready to take the leap? Sign up now to access the latest trading tools and stay at the forefront of the financial markets.
                                                    </p>

                                                    <div className="btn-box">
                                                        <Link className="btn1" to="/signup">Sign Up<span className="sr-only">(current)</span></Link>
                                                    </div>
                                                </div>
                                            ) : (
                                                    <div>
                                                        <p>
                                                            Embrace the future of trading with Trade Synergies. Our advanced technology empowers you to make informed decisions and stay ahead in the market.
                                                        </p>

                                                        <div className="btn-box">
                                                            <Link className="btn1" to="/oi">Discover Technology<span className="sr-only">(current)</span></Link>
                                                        </div>
                                                    </div>

                                            )}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="carousel-item">
                        <div className="container">
                            <div className="row">
                                <div className="col-md-7">
                                    <div className="detail-box">
                                        <div>
                                            <h1>
                                                Maximize Profits with Our Intelligent Trading Solutions
                                            </h1>

                                            

                                            {!isLoggedIn ? (
                                                <div>
                                                    <p>
                                                        Ready to boost your returns? Join us now and experience the power of intelligent trading with Trade Synergies.
                                                    </p>

                                                    <div className="btn-box">
                                                        <Link className="btn1" to="/signup">Sign Up<span className="sr-only">(current)</span></Link>
                                                    </div>
                                                </div>
                                            ) : (
                                            
                                                    <div>
                                                        <p>
                                                            Elevate your profits with Trade Synergies' intelligent trading solutions. Our algorithms analyze market trends to optimize your trading strategies.
                                                        </p>

                                                        <div className="btn-box">

                                                            <Link className="btn1" to="/ordersetting/-1">
                                                                Optimize Strategies<span className="sr-only">(current)</span>
                                                            </Link>
                                                        </div>
                                            </div>
                                            )}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <ol className="carousel-indicators">
                    <li data-target="#customCarousel1" data-slide-to="0" className="active"></li>
                    <li data-target="#customCarousel1" data-slide-to="1"></li>
                    <li data-target="#customCarousel1" data-slide-to="2"></li>
                </ol>
            </div>

        </section>
    
    );
}
export default Home;