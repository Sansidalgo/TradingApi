import React, { useEffect, useState } from 'react';
import { checkTokenExpiration } from './AuthHelpers';
import { Link } from 'react-router-dom';

function OrderSettings() {
    const [apistatus, setApiStatus] = useState('');
    const [settings, setSettings] = useState([]);
    const [pendingDelete, setPendingDelete] = useState(null);
    const [isSmallScreen, setIsSmallScreen] = useState(window.innerWidth <= 1199.98);

    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        if (status) {
            populateOrderSettingsData(user.token);
        } else {
            setApiStatus('Session has expired, Login and retry.');
            throw new Error('Session has expired, Login and retry.');
        }

        window.addEventListener('resize', handleResize);

        return () => {
            window.removeEventListener('resize', handleResize);
        };

    }, []);


    useEffect(() => {
        setIsSmallScreen(window.innerWidth <= 1199.98);
    }, []);

    const handleResize = () => {
        setIsSmallScreen(window.innerWidth <= 1199.98);
    };

    const tableContainerClass = `${isSmallScreen ? 'table-responsive table-responsive-sm' : ' '}`;
    const handleDelete = (id) => {
        setPendingDelete(id);
    };
    const handleTriggerOrder = (id) => {
        const { status, user } = checkTokenExpiration();
        PlaceOrder(user.token, id);
        setApiStatus("OrderPlaced");
    };

    const handleConfirmDelete = () => {
        const { status, user } = checkTokenExpiration();
        deleteOrderSettingsData(user.token, pendingDelete);
        console.log(pendingDelete)
        const updatedSettings = settings.filter((item) => item.id !== pendingDelete);
        setSettings(updatedSettings);
        setPendingDelete(null);


    };

    const handleCancelDelete = () => {
        setPendingDelete(null);
    };

    const contents =
        settings.length === 0 ? (
            <p>
                <em>Loading... Order Settings details</em>
            </p>
        ) : (
            <div className={tableContainerClass}>
                <table className="table table-bordered table-striped">
                    <thead key="idThread">
                        <tr key="trHeader">
                            <th>Id</th>
                            <th>OSID</th>
                            <th>Name</th>
                            <th>Strategy</th>
                            <th>Stock Broker</th>
                            <th>Order Side</th>
                            <th>Credentials</th>
                            <th>Options Settings</th>
                            <th>Environment</th>
                            <th>Trigger</th>
                            <th>Actions</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody key="recordTable">
                        {settings.map((credential, index) => (
                            <tr key={index}>
                                <td>{credential.id}</td>
                                <td>
                                    {`${credential.strategyName}_${credential.instrumentName}_${credential.orderSideName.trim()}_${credential.environmentName}_${credential.id}`.replace(/\s+/g, '_')}
                                </td>

                                <td>{credential.name}</td>
                                <td>{credential.strategyName}</td>
                                <td>{credential.brokerName}</td>
                                <td>{credential.orderSideName}</td>
                                <td>{credential.credentialsName}</td>
                                <td>{credential.optionsSettingsName}</td>
                                <td>{credential.environmentName}</td>
                                <td>
                                    <Link className="nav-link" onClick={() => handleTriggerOrder(credential.id)}>Trigger</Link>
                                </td>
                                <td>
                                    <Link className="nav-link" to={`/ordersetting/${credential.id}`}>
                                        Edit
                                    </Link>
                                </td>
                                <td>
                                    {pendingDelete === credential.id ? (
                                        <div>
                                            <button className="nav-link" onClick={handleConfirmDelete}>Confirm Delete</button>
                                            <button className="nav-link" onClick={handleCancelDelete}>Cancel</button>
                                        </div>
                                    ) : (
                                        <Link className="nav-link" onClick={() => handleDelete(credential.id)}>Delete</Link>
                                    )}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        );

    return (
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-5">
                        <div className="heading_container">
                            <h2>Order Settings</h2>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-11 col-md-12 offset-md-12">
                        {contents ?? (
                            <p>
                                <em>No Order Settings available</em>
                            </p>
                        )}
                        <div className="btn-box">
                            <Link className="btn1" to="/ordersetting/-1">
                                Add Setting<span className="sr-only">(current)</span>
                            </Link>
                        </div>
                        <div>
                            {apistatus }
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );

    async function populateOrderSettingsData(token) {
        const response = await fetch('api/OrderSettings/GetOrderSettings', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`,
            },
        });
        const data = await response.json();
        console.log("settings data")
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

    async function deleteOrderSettingsData(token, id) {
        const response = await fetch(`api/OrderSettings/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`,
            },
        });
        const data = await response.json();

        if (response.ok) {
            // Registration successful
            if (data.status === 1) {

                setApiStatus(data.message);
            } else {
                setApiStatus(data.message);
                console.log(data.message);

            }
        } else {
            // Registration failed
            setApiStatus('Login failed!');
            console.error('Login failed');
            throw new Error('Login failed');
        }
    }
    async function PlaceOrder(token, id) {
        const response = await fetch(`api/ShoonyaNew/ExecuteOrderById?orderSettingId=${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`,
            },
        });
        const data = await response.json();

        if (response.ok) {
            // Registration successful
            if (data.status === 1) {

                setApiStatus(data.message);
            } else {
                setApiStatus(data.message);
                console.log(data.message);

            }
        } else {
            // Registration failed
            setApiStatus('Login failed!');
            console.error('Login failed');
            throw new Error('Login failed');
        }
    }
}

export default OrderSettings;
