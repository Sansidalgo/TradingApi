﻿import React, { useState } from 'react';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCartPlus, faCalculator, faMoneyBill } from '@fortawesome/free-solid-svg-icons';
import { Link } from 'react-router-dom';
import { checkTokenExpiration } from '../AuthHelpers'

const Subscriptions = () => {
   
   
    const [selectedTab, setSelectedTab] = useState(0);

    const [apistatus, setApiStatus] = useState('');
    const [formData, setFormData] = useState({
        PlanName: '',
        PlanTerm: '',
        TransactionId: '',
        Amount: 0,

    });
    const handleSubscribeNow = (planName) => {
        // Switch to the "Payments" tab (index 1)
        setSelectedTab(1);


        let calculatedAmount = 0;
        console.log("selectedSubscriptionType: " + formData.PlanTerm)
        console.log(planName)
        if (planName === 'BASIC') {
            calculatedAmount = formData.PlanTerm === 'MONTHLY' ? 999 : 9999;
        } else if (planName === 'PREMIUM') {
            calculatedAmount = formData.PlanTerm === 'MONTHLY' ? 4999 : 49999;
        }
        console.log(calculatedAmount)
        // Set the calculated amount to the state
        setFormData((prevFormData) => ({
            ...prevFormData,
            Amount: calculatedAmount,
            PlanName: planName
            
        }));



    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const { status, user } = checkTokenExpiration();
            console.log(formData);
            const response = await fetch('/api/Payments/AddPayments', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${user.token}`
                },
                body: JSON.stringify(formData),
            });


            if (response.ok) {
                const data = await response.json();
                // Registration successful

                setApiStatus(data.message);
                console.log('Payment submitted successfully!');
                setApiStatus('Thanks for submitting the payment, will review and activate your subscription in couple of hours')
            } else {
                setApiStatus('Error: adding payment not successfull, please share your transaction id and error screenshot via whatsapp ');
                console.error('Error:Adding payment not successfull, please share your transaction id and error screenshot via whatsapp ');
                throw new Error(data.message);
            }
        } catch (error) {
            setApiStatus('Error while adding payment, please share your transaction id and error screenshot via whatsapp:', error);
            console.error('Error while adding payment, please share your transaction id and error screenshot via whatsapp:', error);
        }

    };
    return (
        <Tabs id="subscriptionsTab" selectedIndex={selectedTab} onSelect={(index) => setSelectedTab(index)}>
            <TabList>
                <Tab>
                    <FontAwesomeIcon icon={faCartPlus} /> Subscriptions
                </Tab>
                <Tab>
                    <FontAwesomeIcon icon={faCalculator} /> Payments
                </Tab>
                <Tab>
                    <FontAwesomeIcon icon={faMoneyBill} /> Payments History
                </Tab>
            </TabList>

            <TabPanel>
                <section className="subscription_section layout_padding-top">
                    <div className="row">

                        {/* Basic Plan content */}
                        <div className="col-md-6 ">
                            <div className="box">
                                <div className="detail-box">
                                    <h5>
                                        <span> Basic Plan: Your Gateway to Simplicity and Connectivity </span>
                                    </h5>

                                    <div className="key-features">
                                        <h3>Key Features:</h3>
                                        <ul>
                                            <li><strong>Single Broker Trading:</strong> Trade effortlessly with a single broker of your choice.</li>
                                            <li><strong>API Access:</strong> Unlock the power of our API to integrate trading functionalities seamlessly into your systems.</li>
                                        </ul>
                                    </div>

                                    <div className="why-choose">
                                        <h3>Why Choose the Basic Plan:</h3>
                                        <ul>
                                            <li><strong>Simplicity:</strong> Focus on your trades without unnecessary complexity.</li>
                                            <li><strong>Broker Flexibility:</strong> Choose from a variety of supported brokers.</li>
                                            <li><strong>API Integration:</strong> Harness the potential of our API to integrate trading features into your systems.</li>
                                        </ul>
                                    </div>

                                    <div className="who-is-for">
                                        <h3>Who Is It For:</h3>
                                        <p>The Basic Plan caters to individuals and traders looking for a straightforward and accessible way to trade with a single broker while enjoying the benefits of API integration.</p>
                                    </div>

                                    <div className="get-started">
                                        <h3>Get Started Today:</h3>
                                        <p>Embark on your trading journey with the Basic Plan. Experience simplicity, connectivity, and the freedom to trade with your preferred broker. Join us today and elevate your trading experience.</p>
                                    </div>

                                    <div className="price-details">
                                        <div className="pricing-item">
                                            <h4>
                                                <input
                                                    type="radio"
                                                    id="monthlybasic"
                                                    name="subscription"
                                                    value="monthly"
                                                    checked={formData.PlanTerm === 'MONTHLY'}
                                                    onChange={() => setFormData({ ...formData, PlanTerm: 'MONTHLY' })}
                                                />
                                                <label htmlFor="monthly">Monthly: ₹999</label>
                                            </h4>
                                            <h4>
                                                <input
                                                    type="radio"
                                                    id="yearlylybasic"
                                                    name="subscription"
                                                    value="yearly"
                                                    checked={formData.PlanTerm === 'YEARLY'}
                                                    onChange={() => setFormData({ ...formData, PlanTerm: 'YEARLY' })}
                                                />
                                                <label htmlFor="yearly">Annual: ₹9999</label>
                                            </h4>
                                        </div>
                                    </div>

                                    <div className="btn-box">
                                        <button
                                            className="btn1"
                                            onClick={() => handleSubscribeNow('BASIC')}
                                        >
                                            Subscribe Now
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        {/* Premium Plan content */}
                        <div className="col-md-6 ">
                            <div className="box">
                                <div className="detail-box">
                                    <h5>
                                        <span> Premium Plan: Elevate Your Trading Experience </span>
                                    </h5>

                                    <div className="key-features">
                                        <h3>Key Features:</h3>
                                        <ul>
                                            <li><strong>Multi-Broker Trading:</strong> Execute trades seamlessly across various brokerage accounts using a unified system.</li>
                                            <li><strong>Trade Copying:</strong> Copy trades effortlessly to replicate successful strategies.</li>
                                            <li><strong>Bulk Modifications:</strong> Perform bulk modifications, cancellations, and square-offs for efficient portfolio management.</li>

                                        </ul>
                                    </div>

                                    <div className="why-choose">
                                        <h3>Why Choose the Premium Plan:</h3>
                                        <ul>
                                            <li><strong>Unified Trading:</strong> Access multiple brokerages through a single, unified system.</li>
                                            <li><strong>Advanced Trading Tools:</strong> Utilize advanced tools for trade copying and portfolio management.</li>
                                            <li><strong>Efficiency and Control:</strong> Manage multiple accounts with ease, making bulk modifications and trade adjustments.</li>

                                        </ul>
                                    </div>

                                    <div className="who-is-for">
                                        <h3>Who Is It For:</h3>
                                        <p>The Premium Plan is designed for experienced traders and institutions seeking advanced trading capabilities. Ideal for those who want to manage multiple brokerage accounts efficiently and enhance their trading strategies.</p>
                                    </div>

                                    <div className="get-started">
                                        <h3>Get Started Today:</h3>
                                        <p>Take your trading to the next level with the Premium Plan. Enjoy the convenience of multi-broker trading, trade copying, and advanced portfolio management. Join us today for a comprehensive and powerful trading experience.</p>
                                    </div>

                                    <div className="price-details">
                                        <div className="pricing-item">
                                            <h4>
                                                <input
                                                    type="radio"
                                                    id="monthlypremium"
                                                    name="subscription"
                                                    value="monthly"
                                                    checked={formData.PlanTerm === 'MONTHLY'}
                                                    onChange={() => setFormData({ ...formData, PlanTerm: 'MONTHLY' })}
                                                />
                                                <label htmlFor="monthly">Monthly: ₹4999</label>
                                            </h4>
                                            <h4>
                                                <input
                                                    type="radio"
                                                    id="yearlypremium"
                                                    name="subscription"
                                                    value="yearly"
                                                    checked={formData.PlanTerm === 'YEARLY'}
                                                    onChange={() => setFormData({ ...formData, PlanTerm: 'YEARLY' })}
                                                />
                                                <label htmlFor="yearly">Annual: ₹49999</label>
                                            </h4>
                                        </div>
                                    </div>

                                    <div className="btn-box">
                                        <button
                                            className="btn1"
                                            onClick={() => handleSubscribeNow('PREMIUM')}
                                        >
                                            Subscribe Now
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </TabPanel>

            <TabPanel>
                <section className="subscription_section layout_padding-top">
                    <div className="row">
                        {/* Content for "Payments" tab */}

                        <div className="col-md-6 ">
                            <div className="box">
                                <div className="detail-box">
                                    <h5>
                                        <span><p>Selected Plan: {formData.PlanName}</p>
                                            <p>Subscription Type: {formData.PlanTerm}</p>
                                            <p>Amount to be paid ₹: {formData.Amount}</p> </span>
                                    </h5>
                                    <div className="form_container">
                                        <form onSubmit={handleSubmit}>
                                            <div>
                                                <div style={{ display: 'flex', flexDirection: 'row' }}>
                                                    <label >Transaction Id :</label>

                                                </div>
                                                <input required
                                                    name="TransactionId"
                                                    value={formData.TransactionId}
                                                    onChange={handleChange}
                                                    type="text"
                                                    placeholder="Transaction Id"
                                                />
                                            </div>

                                            <div>
                                                <div style={{ display: 'flex', flexDirection: 'row' }}>
                                                    <label >Amount (₹) :</label>
                                                  
                                                </div>
                                                <input
                                                    name="Amount"
                                                    value={formData.Amount}
                                                    onChange={handleChange}
                                                    type="text"
                                                    placeholder="Amount"
                                                />
                                            </div>

                                            <div className="btn_box">
                                                <button type="submit">Submit</button>
                                            </div>
                                            <div className="second-div">
                                                <label>Status: {apistatus}</label>
                                            </div>
                                        </form>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div className="col-md-6 ">
                            <div className="box">
                                <div className="img-box">
                                    <img src="images/phonepe.png" alt="" />
                                </div>

                            </div>
                        </div>
                    </div>
                </section>

            </TabPanel>
            <TabPanel></TabPanel>
        </Tabs>
    );
};

export default Subscriptions;
