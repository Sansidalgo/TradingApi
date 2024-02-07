import React, { useEffect, useState } from 'react';
import { checkTokenExpiration } from './authhelpers'


function Notifications() {
    const [apistatus, setApiStatus] = useState('');
    const [delegateNotificationsData, setDelegateNotificationsData] = useState([]);
    const notificationCount = delegateNotificationsData.length;
    const [approvalStatus, setApprovalStatus] = useState({});

    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        if (status) {
            console.log("inside use effect");
            getNotificationDetails(user.token);
        } else {
            setApiStatus('Session has expired, Login and retry.');
            throw new Error('Session has expired, Login and retry.');
        }

    }, []);

    const handleCheckboxChange = (rowId, status) => {
        setApprovalStatus((prevStatus) => ({
            ...prevStatus,
            [rowId]: status,
        }));
    };
    const handleCheckAllChange = (status) => {
        const updatedStatus = {};
        delegateNotificationsData.forEach((row) => {
            updatedStatus[row.id] = status;
        });
        setApprovalStatus(updatedStatus);
    };
    const handleApproveClick = () => {
        const approvedItems = delegateNotificationsData.filter((item) => item.approvalStatus === 'approve');
        // Perform the approval action using the approvedItems array
        console.log('Approved Items:', approvedItems);
        // You can send the data to the server, update the state, etc.
    };

    const handleDenyClick = () => {
        const deniedItems = delegateNotificationsData.filter((item) => item.approvalStatus === 'deny');
        // Perform the denial action using the deniedItems array
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
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data = await response.json();

        if (response.ok) {
            // Get Notificaationnss successful
            if (data.status === 1) {
                setDelegateNotificationsData(data.result);
 
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

                          {notificationCount>0 ? (
                              <div className="table-responsive">
                                  <p>Below are the pending notifications.</p>
                                  <table className="table">
                                      <thead>
                                          <tr>
                                              <th>Request Id</th>
                                              <th>Requestor Id</th>
                                              <th>Requestor Name</th>
                                              <th>Requestor Email</th>
                                              <th>Approve
                                                  <input
                                                      type="checkbox"
                                                      checked={Object.values(approvalStatus).every((status) => status === 'approve')}
                                                      onChange={(e) => handleCheckAllChange(e.target.checked ? 'approve' : null)}
                                                  />

                                              </th>
                                              <th>Deny
                                                  <input
                                                      type="checkbox"
                                                      checked={Object.values(approvalStatus).every((status) => status === 'deny')}
                                                      onChange={(e) => handleCheckAllChange(e.target.checked ? 'deny' : null)}
                                                  />
                                              </th>
                                              
                                          </tr>
                                      </thead>
                                      <tbody>
                                          {delegateNotificationsData.map((notification, index) => (
                                              <tr key={index}>
                                                  <td>{notification.id}</td>
                                                  <td>{notification.traderid}</td>
                                                  <td>{notification.name}</td>
                                                  <td>{notification.email}</td>
                                                  <td><input
                                                      type="checkbox"
                                                      checked={approvalStatus[index] === 'approve'}
                                                      onChange={() => handleCheckboxChange(index, 'approve')}
                                                  /></td>
                                                  <td>
                                                      <input
                                                          type="checkbox"
                                                          checked={approvalStatus[index] === 'deny'}
                                                          onChange={() => handleCheckboxChange(index, 'deny')}
                                                      /></td>
                                              </tr>
                                          ))}
                                      </tbody>
                                      
                                  </table>
                                  <button onClick={handleApproveClick}>Approve Selected</button>
                                  <button onClick={handleDenyClick}>Deny Selected</button>
                              </div>
                          ): (
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