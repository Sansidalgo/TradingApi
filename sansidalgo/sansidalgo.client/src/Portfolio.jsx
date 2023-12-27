import React, { useEffect, useState } from 'react';
import { checkTokenExpiration } from './AuthHelpers';
import { Link } from 'react-router-dom';

function Portfolio() {
    const [apistatus, setApiStatus] = useState('');
    const [orders, setOrders] = useState([]);
    const [totalPnL, setTotalPnL] = useState(0);
   

    useEffect(() => {
        const { status, user } = checkTokenExpiration();
        if (status) {
            populateOrderSettingsData(user.token);
        } else {
            setApiStatus('Session has expired, Login and retry.');
            throw new Error('Session has expired, Login and retry.');
        }
    }, []);

    const contents =
        orders.length === 0 ? (
            <p>
                <em>No Orders available</em>
            </p>
        ) : (
            <>
                <table className="table table-bordered">
                    <thead key="idThread">
                        <tr key="trHeader">
                            <th>Id</th>
                            <th>Instrument</th>
                            <th>Asset</th>
                            <th>Index At</th>
                            
                            <th>Quantity</th>
                            <th>Order Date</th>
                            <th>Order Side</th>

                            <th>Buy At</th>
                            <th>Sell At</th>
                            <th>Environment</th>
                            <th>Segment</th>
                            <th>Source</th>
                        </tr>
                    </thead>
                    <tbody key="recordTable">
                        {orders.map((order, index) => {





                            return (
                                <tr key={index}>
                                    <td>{order.id}</td>
                                    <td>{order.instrumentName}</td>
                                    <td>{order.asset}</td>
                                    <td>{order.indexPriceAt}</td>
                                   
                                    <td>{order.quantity}</td>
                                    <td>{order.createdDt}</td>
                                    <td>{order.orderSideName}</td>
                                    {/* <td>{pnl.toFixed(2)}</td>*/}
                                    <td>{order.buyAt}</td>
                                    <td>{order.sellAt}</td>
                                    <td>{order.environmentName}</td>
                                    <td>{order.segmentName}</td>
                                    <td>{order.orderSourceName}</td>
                                </tr>
                            );
                        })}
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colSpan="8"></td>
                                <td>Total Buy P&L: {totalPnL.toFixed(2)}</td>
                                <td colSpan="4"></td>
                            </tr>

                        </tfoot>
                </table>
              


            </>
        );

    return (
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <div className="row">
                    <div className="col-lg-4 col-md-5 offset-md-1">
                        <div className="heading_container">
                            <h2>Portfolio</h2>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-11 col-md-11 offset-md-1">
                        {contents}
                        <div className="second-div">
                            <label>Status: {apistatus}</label>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );

    async function populateOrderSettingsData(token) {
        const response = await fetch('api/Order/GetOrders', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`,
            },
        });
        const data = await response.json();
        console.log("orders data")
        console.log(data);
        if (response.ok) {
            if (data.status === 1) {
                setOrders(data.result);
                // Calculate totals
               
                let totalPL = 0;
                data.result.forEach(order => {
                    console.log(order.buyAt)
                    console.log(order.sellAt)
                    if (order.buyAt !== undefined && order.sellAt !== undefined && order.buyAt !== 0 && order.sellAt !== 0 && order.buyAt !== null && order.sellAt !== null) {
                        const pnl = order.quantity * order.sellAt - order.quantity * order.buyAt;
                        totalPL += pnl;
                    } 
                    
                });
                setTotalPnL(totalPL);
          
                setApiStatus(data.message);
            } else {
                setApiStatus('Error: ' + data.message);
            }
        } else {
            setApiStatus('Unknown Error!');
            throw new Error('Unknown Error');
        }
    }
}

export default Portfolio;
