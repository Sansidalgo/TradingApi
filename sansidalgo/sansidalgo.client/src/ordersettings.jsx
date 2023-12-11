import React from 'react'
import { useEffect, useState } from 'react';
import { checkTokenExpiration } from './authhelpers'
import { Link } from 'react-router-dom';

function ordersettings() {


    const [apistatus, setApiStatus] = useState("");
    const [settings, setSettings] = useState();

    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        if (status) {
            populateOptionsSettingsData(user.token);
        }
        else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }

    }, []);
    

    const contents = settings === undefined
        ? (
            <p>
                <em>Loading... Please refresh once the ASP.NET backend has started. See
                    <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a>
                    for more details.
                </em>
            </p>
        )
        : (
            <table className="table table-bordered">
                <thead key="idThread">
                    <tr key="trHeader">
                        <th>Id</th>
                        <th>Stock Broker</th>
                        <th>CredentialsName</th>
                        <th>OptionsSettingsName</th>
                       
                    </tr>
                </thead>
                <tbody key="recordTable">
                    {settings?.map((credential, index) => (
                        <tr key={index}>
                            <td>{credential.id}</td>
                            <td>{credential.broker}</td>
                            <td>{credential.credentialsName}</td>
                            <td>{credential.optionsSettingsName}</td>
                           
                        </tr>
                    ))}
                </tbody>
            </table>
        );

    return (
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-1">
                        <div className="heading_container">
                            <h2>Order Settings</h2>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-11 col-md-11 offset-md-1">


                        {contents}




                        <div className="btn-box">
                            <Link className="btn1" to="/ordersetting">Add Setting<span className="sr-only">(current)</span></Link>


                        </div>
                    </div>
                </div>
            </div>
        </section>
    );

    async function populateOptionsSettingsData(token) {

        const response = await fetch('api/OrderSettings/GetOrderSettings', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        const data = await response.json();

        console.log(data);
        if (response.ok) {
            // Registration successful
            if (data.status === 1) {
                setSettings(data.result);
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
export default ordersettings;