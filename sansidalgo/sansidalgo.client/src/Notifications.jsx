import React, { useEffect, useState } from 'react';
import { checkTokenExpiration } from './authhelpers'


function Notifications({ handleNotifications }  ) {
    const [apistatus, setApiStatus] = useState('');
    const [delegateNotificationsData, setDelegateNotificationsData] = useState([]);
    const [notificationCount, setNotificationCount] = useState(0);
    const [approvalStatus, setApprovalStatus] = useState({});
    const [isApproved ,setIsApproved] = useState(false);
    const [isDenied, setIsDenied] = useState(false);
    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        if (status) {
            console.log("inside use effect");
             getNotificationDetails(user.token);
             handleNotificationsLocal();
        } else {
            setApiStatus('Session has expired, Login and retry.');
            throw new Error('Session has expired, Login and retry.');
        }

    }, []);

    const handleNotificationsLocal = async () => {
        try {

            handleNotifications(notificationCount);

        } catch (error) {
            console.error('Error during login:', error);
        }
    };


    const handleCheckboxChange = (index) => {
        setDelegateNotificationsData((prevData) => {
            const newData = [...prevData];
            newData[index].isActive = !newData[index].isActive; // Toggle the isActive property
            return newData;
        });
    };

    const handleCheckAllChange = (status) => {
        setDelegateNotificationsData((prevData) => {
            const newData = prevData.map((row) => ({
                ...row,
                isActive: status,
            }));
            return newData;
        });
    };

    const handleApproveClick = () => {
        const approvedItems = delegateNotificationsData.filter((item) => item.isActive === true);
        // Perform the approval action using the approvedItems array
        if (approvedItems.length > 0) {
            setIsApproved(true);
        }
        console.log('Approved Items:', approvedItems);
        // You can send the data to the server, update the state, etc.
    };

    const handleDenyClick = () => {
        const deniedItems = delegateNotificationsData.filter((item) => item.isActive === false);
        // Perform the denial action using the deniedItems array
        if (deniedItems.length > 0) {
            setIsDenied(true);
        }
        console.log('Denied Items:', deniedItems);
        // You can send the data to the server, update the state, etc.
    };

    async function getNotificationDetails(token) {
        const response = await fetch(`/api/Notifications/GetNotifications`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });
        if (!response.ok) {
            setApiStatus("Unknown HTTP Error");
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data = await response.json();

        if (response.ok) {
            // Get Notificaationnss successful
            if (data.status === 1) {
                console.log("status approval")
                console.log(data.result);
                setDelegateNotificationsData(data.result);
                setNotificationCount(data.result.length)


            }
            else if (data.status === 0) {
                setApiStatus(data.message);
                console.log(data.message);
            }
            else {
                setApiStatus('Error: ' + data.message);
                console.error('Error:', data.message);
                throw new Error(data.message);
            }
        } else {
            // GetDelegates failed
            setApiStatus('Get Notifications failed!');
            console.error('Get Notifications failed');
            throw new Error('Get Notifications failed');
        }
    }


    return (
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-1">
                        <div className="heading_container">
                            <h2>Notifications</h2>

                            {notificationCount > 0 ? (
                                <div className="table-responsive">
                                    <p>Below are the pending notifications.</p>
                                    <table className="table">
                                        <thead>
                                            <tr>
                                                {/*<th>Request Id</th>*/}
                                                <th>Requestor Id</th>
                                                <th>Requestor Name</th>
                                                <th>Requestor Email</th>
                                                <th>Approve
                                                    <input
                                                        type="checkbox"
                                                        checked={delegateNotificationsData.every((row) => row.isActive)}
                                                        onChange={(e) => handleCheckAllChange(e.target.checked)}
                                                    />
                                                </th>

                                                <th>Deny
                                                    <input
                                                        type="checkbox"
                                                        checked={delegateNotificationsData.every((row) => !row.isActive)}
                                                        onChange={(e) => handleCheckAllChange(!e.target.checked)}
                                                    />
                                                </th>
                                                
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {delegateNotificationsData.map((notification, index) => (
                                                <tr key={index}>
                                                    {/* <td>{notification.id}</td>*/}
                                                    <td>{notification.traderId}</td>
                                                    <td>{notification.name}</td>
                                                    <td>{notification.emailId}</td>
                                                    <td>
                                                        <input
                                                            type="checkbox"
                                                            checked={notification.isActive}
                                                            onChange={() => handleCheckboxChange(index)}
                                                        />
                                                    </td>
                                                    <td>
                                                        <input
                                                            type="checkbox"
                                                            checked={!notification.isActive}
                                                            onChange={() => handleCheckboxChange(index)}
                                                        />
                                                    </td>
                                                </tr>
                                            ))}
                                        </tbody>
                                    </table>
                                    <button onClick={handleApproveClick}>Approve Selected</button>
                                    <button onClick={handleDenyClick}>Deny Selected</button>
                                </div>
                            ) : (
                                <><p>You dont have any notifications at the moment.</p></>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default Notifications;