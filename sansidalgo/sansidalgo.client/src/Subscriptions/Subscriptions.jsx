import React, { useState, useEffect } from 'react';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHome, faUser, faCog } from '@fortawesome/free-solid-svg-icons';
import { faCalculator, faCartPlus, faPlane, faPray } from '../../node_modules/@fortawesome/free-solid-svg-icons/index';
import DropDown from '../DropDown';

import { Link } from 'react-router-dom';

const Subscriptions = () => {
    //initialazing variables
    const [selectedPlanId, setSelectedPlanId] = useState(0);
    const [apistatus, setApiStatus] = useState("");
    const [formData, setFormData] = useState({
        planId: 0
    });
    //end initialazing variables

    //*form validation logic*/
    const validateForm = () => {
        let valid = true;
        const errors = {
            planId: '*'
        };
        return valid;
    };
    const [formErrors, setFormErrors] = useState({
        planId: '*'
    });
    //*end form validation logic*/

    //select plan logic
    const handlePlanApiStatusChange = (newApiStatus) => {
        setApiStatus(newApiStatus);
    };
    const handlePlanSelectedOptionChange = (selectedOption) => {
        setFormData((prevFormData) => ({
            ...prevFormData,
            planId: selectedOption
        }));
        setFormErrors({ ...formErrors, planId: "*" });
    };
    //end select plan logic

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validateForm()) {

            setApiStatus("Form validation failed, please check the validations and try again");
            console.log('Form validation failed');
            return;
        };
    };

    return (
        <Tabs >
            <TabList>
                <Tab> <FontAwesomeIcon icon={faCartPlus} /> Subscriptions </Tab>
                <Tab> <FontAwesomeIcon icon={faCalculator} /> Payments</Tab>
                {/* Add more Tab components as needed */}
            </TabList>

            <TabPanel>

                <section className="subscription_section layout_padding-top">
                    <div className="service_container">
                        <div className="container ">
                            <div className="heading_container">
                                <h2>
                                    <span>Plans</span>
                                </h2>
                                <p>
                                    Welcome to our platform! We understand that everyone has unique needs, and that's why we offer a range of plans tailored to suit you. Whether you're an individual trader or part of a larger organization, we have the right plan to empower your trading experience.
                                </p>
                            </div>
                            <div className="row">
                                <div className="col-md-6 ">
                                    <div className="box">

                                        <div className="detail-box">
                                            <h5><span> Basic Plan: Your Gateway to Simplicity and Connectivity </span></h5>


                                            <div class="key-features">
                                                <h3>Key Features:</h3>
                                                <ul>
                                                    <li><strong>Single Broker Trading:</strong> Trade effortlessly with a single broker of your choice.</li>
                                                    <li><strong>API Access:</strong> Unlock the power of our API to integrate trading functionalities seamlessly into your systems.</li>
                                                </ul>
                                            </div>

                                            <div class="why-choose">
                                                <h3>Why Choose the Basic Plan:</h3>
                                                <ul>
                                                    <li><strong>Simplicity:</strong> Focus on your trades without unnecessary complexity.</li>
                                                    <li><strong>Broker Flexibility:</strong> Choose from a variety of supported brokers.</li>
                                                    <li><strong>API Integration:</strong> Harness the potential of our API to integrate trading features into your systems.</li>
                                                </ul>
                                            </div>

                                            <div class="who-is-for">
                                                <h3>Who Is It For:</h3>
                                                <p>The Basic Plan caters to individuals and traders looking for a straightforward and accessible way to trade with a single broker while enjoying the benefits of API integration.</p>
                                            </div>

                                            <div class="get-started">
                                                <h3>Get Started Today:</h3>
                                                <p>Embark on your trading journey with the Basic Plan. Experience simplicity, connectivity, and the freedom to trade with your preferred broker. Join us today and elevate your trading experience.</p>
                                            </div>
                                            <div class="price-details">

                                                <div class="pricing-item">
                                                    <h4>
                                                        <p>Monthly: ₹999</p>
                                                        <p>Annual: ₹9999</p>
                                                    </h4>
                                                </div>


                                            </div>
                                            <div className="btn-box">
                                                <Link className="btn1" to="/signup">Subscribe Now<span className="sr-only">(current)</span></Link>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-md-6 ">
                                    <div className="box">

                                        <div className="detail-box">
                                            <h5>
                                                <span>
                                                    Premium Plan: Elevate Your Trading Experience
                                                </span>
                                            </h5>
                                            <div class="key-features">
                                                <h3>Key Features:</h3>
                                                <ul>
                                                    <li><strong>Multi-Broker Trading:</strong> Execute trades seamlessly across various brokerage accounts using a unified system.</li>
                                                    <li><strong>Trade Copying:</strong> Copy trades effortlessly to replicate successful strategies.</li>
                                                    <li><strong>Bulk Modifications:</strong> Perform bulk modifications, cancellations, and square-offs for efficient portfolio management.</li>

                                                </ul>
                                            </div>

                                            <div class="why-choose">
                                                <h3>Why Choose the Premium Plan:</h3>
                                                <ul>
                                                    <li><strong>Unified Trading:</strong> Access multiple brokerages through a single, unified system.</li>
                                                    <li><strong>Advanced Trading Tools:</strong> Utilize advanced tools for trade copying and portfolio management.</li>
                                                    <li><strong>Efficiency and Control:</strong> Manage multiple accounts with ease, making bulk modifications and trade adjustments.</li>

                                                </ul>
                                            </div>

                                            <div class="who-is-for">
                                                <h3>Who Is It For:</h3>
                                                <p>The Premium Plan is designed for experienced traders and institutions seeking advanced trading capabilities. Ideal for those who want to manage multiple brokerage accounts efficiently and enhance their trading strategies.</p>
                                            </div>

                                            <div class="get-started">
                                                <h3>Get Started Today:</h3>
                                                <p>Take your trading to the next level with the Premium Plan. Enjoy the convenience of multi-broker trading, trade copying, and advanced portfolio management. Join us today for a comprehensive and powerful trading experience.</p>
                                            </div>
                                            <div class="price-details">

                                                <div class="pricing-item">
                                                    <h4>
                                                        <p>Monthly: ₹4999</p>
                                                        <p>Annual: ₹49999</p>
                                                    </h4>
                                                </div>

                                            </div>
                                            <div className="btn-box">
                                                <Link className="btn1" to="/signup">Subscribe Now<span className="sr-only">(current)</span></Link>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </section>

            </TabPanel>
            <TabPanel>
                {/* Content for Tab 2 */}
                <p>Content for Tab 2</p>
            </TabPanel>
            {/* Add more TabPanel components as needed */}
        </Tabs>
    );
};

export default Subscriptions;
