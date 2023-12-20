import React, { useEffect, useState } from 'react';
import { checkTokenExpiration } from './AuthHelpers';
import { Link } from 'react-router-dom';

function Portfolio() {
    const [apistatus, setApiStatus] = useState('');
    const [orders, setOrders] = useState([]);
    const [buyTotal, setBuyTotal] = useState(0);
    const [sellTotal, setSellTotal] = useState(0);

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
                            <th>Price</th>
                            <th>Quantity</th>
                            <th>Order Side</th>
                            <th>P&L</th>
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
                                    <td>{order.price}</td>
                                    <td>{order.quantity}</td>
                                    <td>{order.orderSideName}</td>
                                    {/* <td>{pnl.toFixed(2)}</td>*/}
                                   <td></td>
                                    <td>{order.environmentName}</td>
                                    <td>{order.segmentName}</td>
                                    <td>{order.orderSourceName}</td>
                                </tr>
                            );
                        })}
                    </tbody>
                    </table>
                    <tfoot>
                        <tr>
                            <td colSpan="6"></td>
                            <td>Total Buy P&L: {buyTotal.toFixed(2)}</td>
                            <td colSpan="3"></td>
                        </tr>
                        <tr>
                            <td colSpan="6"></td>
                            <td>Total Sell P&L: {sellTotal.toFixed(2)}</td>
                            <td colSpan="3"></td>
                        </tr>
                        <tr>
                            <td colSpan="6"></td>
                            <td>Total P&L: {(buyTotal - sellTotal).toFixed(2)}</td>
                            <td colSpan="3"></td>
                        </tr>
                    </tfoot>

           
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
                let totalBuy = 0;
                let totalSell = 0;

                data.result.forEach(order => {
                    const pnl = order.quantity * order.price;
                    if (order.orderSideName.toLowerCase() === 'pebuy') {
                        totalBuy += pnl;
                    } else {
                        totalSell += pnl;
                    }
                });

                setBuyTotal(totalBuy);
                setSellTotal(totalSell);
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
