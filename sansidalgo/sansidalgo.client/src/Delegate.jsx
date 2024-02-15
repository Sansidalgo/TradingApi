import React, { useEffect, useState } from 'react';
import { checkTokenExpiration } from './authhelpers'
import { Link } from 'react-router-dom';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
function Delegate() {

    const [userDelegateExists, setUserDelegateExists] = useState(false);
    const [apistatus, setApiStatus] = useState('');
    let initialMasterTrader = { Id: 0, name: '' };
    const [masterTrader, setMasterTrader] = useState(initialMasterTrader);
    const [reloadFlag, setReloadFlag] = useState(false);
    const [masterTraderId, setMasterTraderId] = useState(0);
    const [traderId, setTraderId] = useState(0);
    const [termsChecked, setTermsChecked] = useState(false);
    
    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        if (status) {
            console.log("inside use effect");
            getDelegateDetails(user.token);
        } else {
            setApiStatus('Session has expired, Login and retry.');
            throw new Error('Session has expired, Login and retry.');
        }
        
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            await handleAddDelegate();
            //setReloadFlag(!reloadFlag);
        } catch (error) {
            // Handle errors if needed
        }
    }; 
    const handleAddDelegate = async () => {
        const { status, user } = checkTokenExpiration();
        if (status) {
            try {
                console.log('Delegate Master ID:'+masterTraderId);
                const response = await fetch(`api/Delegate/AddDelegate?masterTraderId=${masterTraderId}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${user.token}`
                    },
                    
                });
                const data = await response.json();

                if (response.ok) {
                    // Registration successful

                    setApiStatus(data.message);
                    setMasterTrader(data.result);
                    console.log(masterTrader);
                    console.log('Delegate registered successfully!');
                } else {
                    setApiStatus('Error: ' + data.message);
                    console.error('Error:', data.message);
                    throw new Error(data.message);
                }
            } catch (error) {
                setApiStatus('Error during adding Delegate:', error);
                console.error('Error during adding Delegate:', error);
            }
        } else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }
    };
    const handleChange = (e) => {
        const { name, value } = e.target;
        setMasterTraderId(value);
        //setFormData({ ...formData, [name]: value });
    };

    const handleRemove = async (id) => {
        const { status, user } = checkTokenExpiration();
        console.log("status is::"+status);
        if (status) {
            try {
                console.log("test id:" + id);
                const response = await fetch(`api/Delegate/DeleteDelegate?Id=${id}`, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${user.token}`
                    },
                    
                });
                const data = await response.json();

                if (response.ok) {
                    // Registration successful

                    setApiStatus(data.message);
                    setMasterTrader(initialMasterTrader);
                    console.log('Delegate removed successfully!');
                } else {
                    setApiStatus('Error: ' + data.message);
                    console.error('Error:', data.message);
                    throw new Error(data.message);
                }
            } catch (error) {
                setApiStatus('Error during removing Delegate:', error);
                console.error('Error during removing Delegate:', error);
            }
        } else {
            setApiStatus("Session has expired, Login and retry.");
            throw new Error("Session has expired, Login and retry.");
        }
    };
    async function getDelegateDetails(token) {
        console.log("in get delegate api call");
        const response = await fetch(`/api/Delegate/GetDelegates`, {
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
            // GetDelegates successful
            if (data.status === 1) {
                console.log("successfully got delegate details");
                console.log("biscuit resuult 0" + data.result.id+data.result.name);
                setUserDelegateExists(true);
                setMasterTrader(data.result);
                //setTraderId(data.result.id);
                console.log("name::");
                console.log(masterTrader)
                
            }
            else if (data.status === 0)
            {
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
            setApiStatus('GetDelegates failed!');
            console.error('GetDelegates failed');
            throw new Error('GetDelegates failed');
        }
    }

    const handleCheckboxChange = () => {
        setTermsChecked(!termsChecked);
    };

  return (
      <section className="contact_section layout_padding-top">
          <div className="container-fluid">
              <div className="row">
                  <div className="col-lg-4 col-md-5 offset-md-1">
                      <div className="heading_container">
                          <h2>Delegates</h2>
                      </div>
                  </div>
              </div>
              <div className="row">
                  <div className="col-lg-11 col-md-11 offset-md-1">
                      
                      {userDelegateExists ? (
                          <><p>Your account is delegated to:<b> {masterTrader.name} </b>.<br />
                              Please remove the existing delegate user to nominate a different delegate.</p>
                              {console.log("in link value is::"  + masterTrader.id + masterTrader.name)}
                              {masterTrader.id && (
                                  <Link className="nav-link" onClick={() => handleRemove(masterTrader.id)}>Remove Delegate</Link>
                              )}
                          </>

                      ) : (
                              
                             <><p>You dont have any delegates yet. To nominate a delegate for your account please enter the delegate person trader id and submit.
                                <br /> Once they approve your delegate request all the trades taken by delegate will reflect in your account as well.</p>
                                <form onSubmit={handleSubmit}>
                                      <div>
                                          <TextField id="outlined-basic" label="Delegate ID" variant="outlined" name="masterTraderId"
                                              value={masterTraderId}
                                              onChange={handleChange}
                                               />
                                        {/* <input*/}
                                        {/*name="masterTraderId"*/}
                                        {/*value={masterTraderId}*/}
                                        {/*onChange={handleChange}*/}
                                        {/*type="text"*/}
                                        {/*placeholder="Delegate Trader Id"*/}
                                        {/*/>*/}
                                      </div>
                                      <div><br /> <label><p>
                                          <input
                                              type="checkbox"
                                              checked={termsChecked}
                                              onChange={handleCheckboxChange}
                                              
                                          />
                                         &nbsp; I Agree to delegate my account. </p>
                                      </label>
                                      <br /></div>
                                      <div className={` ${termsChecked ? 'form_container' : 'undefined'}`}>
                                          <div className="btn_box" style={{ width: '50%' }}>
                                              <button type="submit" disabled={!termsChecked} >Submit</button>
                                          </div></div>
                      
                                 </form>
                            </>
                          )}
                      
                      <div className="second-div">
                          <label>Status: {apistatus}</label>
                      </div>
                      <div className="btn-box">
                          {/*<Link className="btn1" to="/Home">Home</Link>*/}


                      </div>
                  </div>
              </div>
          </div>
      </section>
    );

   
}

export default Delegate;