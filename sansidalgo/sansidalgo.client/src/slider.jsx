import React from 'react'
import { Link } from 'react-router-dom';


function slider() {
    return (
        <section className="slider_section ">
            <div className="slider_bg_box">
                <img src="images/slider-bg.jpg" alt="" />
            </div>
            <div id="customCarousel1" className="carousel slide" data-ride="carousel">
                <div className="carousel-inner">
                    <div className="carousel-item active">
                        <div className="container ">
                            <div className="row">
                                <div className="col-md-7 ">
                                    <div className="detail-box">
                                        <h1>
                                            We Provide best <br />
                                            Trading Services
                                        </h1>
                                        <p>
                                            Ready to transform your trading experience? Sign up today and take the first step towards automated trading success with Trade Synergies.
                                        </p>
                                        <div className="btn-box">
                                            <Link className="btn1" to="/signup">Sign Up<span className="sr-only">(current)</span></Link>

                   
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="carousel-item">
                        <div className="container ">
                            <div className="row">
                                <div className="col-md-7 ">
                                    <div className="detail-box">
                                        <h1>
                                            We Provide best <br />
                                            Transport Service
                                        </h1>
                                        <p>
                                            Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eum magnam, voluptates distinctio, officia architecto tenetur debitis hic aspernatur libero commodi atque fugit adipisci, blanditiis quidem dolorum odit voluptas? Voluptate, eveniet?
                                        </p>
                                        <div className="btn-box">
                                            <a href="" className="btn1">
                                                Get A Quote
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="carousel-item">
                        <div className="container ">
                            <div className="row">
                                <div className="col-md-7 ">
                                    <div className="detail-box">
                                        <h1>
                                            We Provide best <br />
                                            Transport Service
                                        </h1>
                                        <p>
                                            Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eum magnam, voluptates distinctio, officia architecto tenetur debitis hic aspernatur libero commodi atque fugit adipisci, blanditiis quidem dolorum odit voluptas? Voluptate, eveniet?
                                        </p>
                                        <div className="btn-box">
                                            <a href="" className="btn1">
                                                Get A Quote
                                            </a>
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
export default slider;