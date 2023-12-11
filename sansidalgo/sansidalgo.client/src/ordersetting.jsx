import React, { useState, useEffect } from 'react';
import Dropdown from './dropdown'
import Credentials from './credentials'
import OptionsSettings from './optionssettings'
import { checkTokenExpiration } from './authhelpers';


function ordersetting() {
    const intialCredential = {

        Name: '',
        Uid: '',
        Password: '',
        AuthSecreteKey: '',
        Imei: '',
        Vc: '',
        ApiKey: '',
        IsActive: true
    };
    const initialOptionSettings = {
        Name:'',
        Instrument: '',
        ExpiryDay: '',
        LotSize: '',
        CeSideEntryAt: '',
        PeSideEntryAt: ''

    };
    const [formData, setFormData] = useState({

        BrokerId: 0,
        CredentialsID: 0,
        OptionsSettingsId: 0, // Add other common form fields here
        Credential: intialCredential,
        OptionsSetting: initialOptionSettings
    });
    const [apistatus, setApiStatus] = useState("");
    const handleBrokerApiStatusChange = (newApiStatus) => {
        setApiStatus(newApiStatus);
    };
    const handleCredentialApiStatusChange = (newApiStatus) => {
        setApiStatus(newApiStatus);
    };
    const handleOptionsSettingsApiStatusChange = (newApiStatus) => {
        setApiStatus(newApiStatus);
    };

    const handleBrokerSelectedOptionChange = (selectedOption) => {
        setFormData((prevFormData) => ({
            ...prevFormData,
            BrokerId: selectedOption
        }));
    };
    const handleCredentialSelectedOptionChange = (selectedOption) => {
        setFormData((prevFormData) => ({
            ...prevFormData,
            CredentialsID: selectedOption
        }));
    };

    const handleOptionSettingsSelectedOptionChange = (selectedOption) => {
        setFormData((prevFormData) => ({
            ...prevFormData,
            OptionsSettingsId: selectedOption
        }));
    };


    const handleCredentialsFormChange = (credentialFormData) => {
        setFormData((prevFormData) => ({
            ...prevFormData,
            Credential: { ...prevFormData.Credential, ...credentialFormData }
        }));
    };

    const handleOptionsSettingsFormChange = (optionsSettingsFormData) => {
        setFormData((prevFormData) => ({
            ...prevFormData,
            OptionsSetting: { ...prevFormData.OptionsSetting, ...optionsSettingsFormData }
        }));
    };


    const { status, user } = checkTokenExpiration();

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (status) {
            try {
                console.log(formData);
                const response = await fetch('api/OrderSettings/add', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${user.token}`
                    },
                    body: JSON.stringify(formData),
                });
                const data = await response.json();

                if (response.ok) {
                    // Registration successful

                    setApiStatus(data.message);
                    console.log('User registered successfully!');
                } else {
                    setApiStatus('Error: ' + data.message);
                    console.error('Error:', data.message);
                    throw new Error(data.message);
                }
            } catch (error) {
                setApiStatus('Error during adding shoonya credentials:', error);
                console.error('Error during adding shoonya credentials:', error);
            }
        } else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }
    };



    return (

        /* < !--sign up section-- >*/
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-1">
                        <div className="heading_container">
                            <h2>
                                Trade Settings
                            </h2>
                        </div>
                    </div>
                </div>

                <div className="row">
                    <div className="col-lg-6 col-md-11 offset-md-1">
                        <div className="form_container">
                            <form onSubmit={handleSubmit} type="submit">
                                <Dropdown
                                    apiPath="api/Brokers/GetBrokers"
                                    placeholder="Select Stock Broker"
                                    displayProperty="broker"
                                    valueProperty="id"
                                    onApiStatusChange={handleBrokerApiStatusChange}
                                    onSelectedOptionChange={handleBrokerSelectedOptionChange}
                                />
                                <div className="boxTypeDiv">
                                    <Dropdown
                                        apiPath="api/ShoonyaCredentials/GetCredentials"
                                        placeholder="Select Credential"
                                        displayProperty="name"
                                        valueProperty="id"
                                        onApiStatusChange={handleCredentialApiStatusChange}
                                        onSelectedOptionChange={handleCredentialSelectedOptionChange}
                                    />
                                    <div className="orSection">
                                        <label>Or Fill Below Form</label>
                                    </div>

                                    <Credentials onFormChange={handleCredentialsFormChange} />
                                </div>
                                <div className="orSection">
                                    <label>And</label>
                                </div>
                                <div className="boxTypeDiv">
                                    <Dropdown
                                        apiPath="api/OptionsSettings/GetOptionsSettings"
                                        placeholder="Select Option/Future Setting"
                                        displayProperty="name"
                                        valueProperty="id"
                                        onApiStatusChange={handleOptionsSettingsApiStatusChange}
                                        onSelectedOptionChange={handleOptionSettingsSelectedOptionChange}
                                    />
                                    <div className="orSection">
                                        <label>Or Fill Below Form</label>
                                    </div>
                                    <OptionsSettings onFormChange={handleOptionsSettingsFormChange} />
                                </div>

                                <button type="submit">Submit</button>
                            </form>
                            <div className="second-div">
                                <label>Status: {apistatus}</label>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>
        /*  <!--end sign up section-- >*/
    );



}

export default ordersetting;
