import React from 'react'
import ReactDOM from 'react-dom/client'
import { useEffect, useState } from 'react';
import { checkTokenExpiration } from './authhelpers'
import { Link } from 'react-router-dom';

function stockbrokers() {

    const { status, user } = checkTokenExpiration();
    const [apistatus, setApiStatus] = useState("");
    const [shoonyaCredentials, setshoonyaCredentials] = useState();

    useEffect(() => {
        if (status) {
            populateShoonyaCredentialsData();
        }
        else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }

    }, []);

    const contents = shoonyaCredentials === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-bordered">
            <thead key="idThread">
                <tr>
                <th>Id</th>
                    <th>Trader Id</th>
                    <th>Uid</th>
                    <th>AuthSecreteKey</th>
                    <th>Imei</th>
                    <th>Vc</th>
                    <th>ApiKey</th>
                    <th>IsActive</th>

                </tr>
            </thead>
            <tbody key="recordTable">
                {shoonyaCredentials.map(credential =>
                    <tr key={credential.Id}>
                        <td>{credential.TraderId}</td>
                        <td>{credential.Uid}</td>
                        <td>{credential.AuthSecreteKey}</td>
                        <td>{credential.Imei}</td>

                        <td>{credential.Vc}</td>
                        <td>{credential.ApiKey}</td>
                        <td>{credential.IsActive}</td>
                       
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-1">
                        <div className="heading_container">
                            <h2>Stock Brokers</h2>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-11 col-md-11 offset-md-1">


                        {contents}




                        <div className="btn-box">
                            <Link className="btn1" to="/shoonya">Add Broker<span className="sr-only">(current)</span></Link>


                        </div>
                    </div>
                </div>
            </div>
        </section>
    );

    async function populateShoonyaCredentialsData() {

        const response = await fetch('api/ShoonyaCredentials/GetCredentials', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${user.token}`
            }
        });

        const data = await response.json();
        

        if (response.ok) {
            // Registration successful
            if (data.status === 1) {
                setshoonyaCredentials(data.result);
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
export default stockbrokers;