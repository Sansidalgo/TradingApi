import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Dropdown from './dropdown'
import Credentials from './credentials'
import OptionsSettings from './optionssettings'
import { checkTokenExpiration } from './authhelpers';



function ordersetting() {
    // Access the orderSettingId from the URL parameters
    const { orderSettingId } = useParams();
    const [selectedBrokerId, setSelectedBrokerId] = useState(0);
    const [initialCredential, setIntialCredential] = useState({

        name: '',
        uid: '',
        password: '',
        authSecreteKey: '',
        imei: '',
        vc: '',
        apiKey: '',
        isActive: true
    });

    const [initialOptionSettings, setInitialOptionSettings] = useState({
       
        instrument: '',
        expiryDay: '',
        lotSize: 0,
        ceSideEntryAt: 0,
        peSideEntryAt: 0,

    });
    const [formData, setFormData] = useState({

        BrokerId: selectedBrokerId,
        CredentialsID: 0,
        OptionsSettingsId: 0, // Add other common form fields here
        Credential: initialCredential,
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




    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        console.log("order setting" + orderSettingId);
        console.log(status);
        if (status) {
            if (orderSettingId && orderSettingId >= 0) {
                populateOrderSettingsData(user.token, orderSettingId);
            }
        } else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }
    }, []);


    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const { status, user } = checkTokenExpiration();
            console.log(formData);
            const response = await fetch('/api/OrderSettings/add', {
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
                console.log('User registered successfully!');
            } else {
                setApiStatus('Error: Add user not successfull ');
                console.error('Error: Add user not successfull ');
                throw new Error(data.message);
            }
        } catch (error) {
            setApiStatus('Error during adding shoonya credentials:', error);
            console.error('Error during adding shoonya credentials:', error);
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
                                    apiPath="/api/Brokers/GetBrokers"
                                    placeholder="Select Stock Broker"
                                    displayProperty="broker"
                                    valueProperty="id"
                                    onApiStatusChange={handleBrokerApiStatusChange}
                                    onSelectedOptionChange={handleBrokerSelectedOptionChange}
                                    selectedItemId={selectedBrokerId}
                                />
                                <div className="boxTypeDiv">
                                    <Dropdown
                                        apiPath="/api/ShoonyaCredentials/GetCredentials"
                                        placeholder="Select Credential"
                                        displayProperty="name"
                                        valueProperty="id"
                                        onApiStatusChange={handleCredentialApiStatusChange}
                                        onSelectedOptionChange={handleCredentialSelectedOptionChange}

                                    />
                                    <div className="orSection">
                                        <label>Or Fill Below Form</label>
                                    </div>

                                    <Credentials onFormChange={handleCredentialsFormChange} initialValues={initialCredential} />
                                </div>
                                <div className="orSection">
                                    <label>And</label>
                                </div>
                                <div className="boxTypeDiv">
                                    <Dropdown
                                        apiPath="/api/OptionsSettings/GetOptionsSettings"
                                        placeholder="Select Option/Future Setting"
                                        displayProperty="instrument"
                                        valueProperty="id"
                                        onApiStatusChange={handleOptionsSettingsApiStatusChange}
                                        onSelectedOptionChange={handleOptionSettingsSelectedOptionChange}

                                    />
                                    <div className="orSection">
                                        <label>Or Fill Below Form</label>
                                    </div>
                                    <OptionsSettings onFormChange={handleOptionsSettingsFormChange} initialValues={initialOptionSettings} />
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

    async function populateOrderSettingsData(token, orderSettingId) {
        const response = await fetch(`/api/OrderSettings/GetOrderSettingsById?orderSettingId=${orderSettingId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data = await response.json();

        if (response.ok) {
            // Registration successful
            if (data.status === 1) {
                console.log("setting intial dta");
                console.log(data)
                setIntialCredential(data.result.credential);
                setInitialOptionSettings(data.result.optionsSetting);
                setSelectedBrokerId(data.result.brokerDetails.id)

                setApiStatus(data.message);
            } else {
                setApiStatus('Error: ' + data.message);
                console.error('Error:', data.message);
                throw new Error(data.message);
            }
        } else {
            // Registration failed
            setApiStatus('Login failed!');
            console.error('Login failed');
            throw new Error('Login failed');
        }
    }

}

export default ordersetting;
