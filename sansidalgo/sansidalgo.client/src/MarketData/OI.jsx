import React, { useEffect, useState } from 'react';

const OI = () => {
    const [apistatus, setApiStatus] = useState('');
    const [oiData, setOiData] = useState([]);

   

    useEffect(() => {
       
        PopulateOIData();

        // Set up interval to refresh at exact clock times within the time range
        const intervalId = setInterval(() => {
            const currentDateTime = new Date();
            const currentHour = currentDateTime.getUTCHours() + 5; // Add 5 hours to convert to Indian time
           
            // Check if it's within the time range of 9 AM to 3:30 PM IST
            if (currentHour >= 9 && currentHour < 24 && currentDateTime.getMinutes() ===5) {
                console.log("Refreshing...");
                PopulateOIData();
            }
        }, 60 * 1000); // Check every minute

        // Cleanup the interval when the component is unmounted
        return () => clearInterval(intervalId);
    }, []); // Empty dependency array to run the effect only once on mount


    // Function to get prediction notes based on the data
    const getPredictionNotes = () => {
        if (oiData.length > 0) {
            const sortedData = [...oiData].sort((a, b) => new Date(b.entryDateTime) - new Date(a.entryDateTime));
            const latestData = sortedData[0];

            const pcrOiPrediction = getPredictionDiv("PCR OI", latestData.pcrOi);
            const pcrOichangePrediction = getPredictionDiv("PCR OI Change", latestData.pcrOichange);

            return (
                <div style={{ display: 'flex' }}>
                    {pcrOiPrediction}
                    {pcrOichangePrediction}
                </div>
            );
        }
        return null;
    };

    // Function to get prediction div based on the value
    const getPredictionDiv = (label, value) => {
        let backgroundColor, text;
        

        if (value <= 0.8) {
            backgroundColor = `rgba(255, 0, 0, ${Math.min(1, Math.abs(value - 1))}`;
            text = `${label} Prediction: Bearish Trend`;
        } else if (value >= 1.2) {
           
            backgroundColor = `rgba(0, 255, 0, ${Math.min(1, Math.abs(1 - value))})`;
            text = `${label} Prediction: Bullish Trend`;
        } else {
            const opacity = Math.min(1, Math.abs(value - 1));
            backgroundColor = `rgba(128, 128, 128, ${Math.min(1, Math.abs(value - 1))})`;
            text = `${label} Prediction: Sideways Trend`;
        }

        return (
            <div style={{ backgroundColor, padding: '10px', borderRadius: '5px', marginRight: '10px' }}>
                {text}
            </div>
        );
    };

    const tableStyle = {
        display: 'flex',
        justifyContent: 'space-between',
    };

    const tableColumnStyle = {
        border: '1px solid #ddd',
        padding: '8px',
        backgroundColor: '#f2f2f2',
    };

    const contents =
        oiData.length === 0 ? (
            <p>
                <em>Loading... Option chain details</em>
            </p>
        ) : (
            <div className="table-responsive" style={tableStyle}>
                {/* Table 1: Time, PCR OI, PCR OI Change */}
                <table className="table" style={{ width: '48%', ...tableColumnStyle }}>
                    <thead>
                        <tr>
                           {/* <th>ID</th>*/}
                            <th>Time</th>
                            <th>PCR OI</th>
                            <th>PCR OI Change</th>
                        </tr>
                    </thead>
                    <tbody>
                        {oiData.map(item => (
                            <tr key={item.id}>
                              {/*  <td>{item.id}</td>*/}
                                <td>{item.entryDateTime}</td>
                                <td
                                    style={{
                                        backgroundColor: item.pcrOi <= 0.8 
                                            ? `rgba(255, 0, 0, ${Math.min(1, Math.abs(item.pcrOi - 1))})`
                                            : item.pcrOi >= 1.2
                                                ? `rgba(0, 255, 0, ${Math.min(1, Math.abs( 1 - item.pcrOi))})`
                                                : `rgba(128, 128, 128, ${Math.min(1, Math.abs(item.pcrOi - 1))})`, // Adjusted for grey color
                                        
                                    }}
                                >
                                    {item.pcrOi}
                                </td>
                                <td
                                    style={{
                                        backgroundColor: item.pcrOichange <= 0.8 
                                            ? `rgba(255, 0, 0, ${Math.min(1, Math.abs( item.pcrOichange - 1))})`
                                            : item.pcrOichange >= 1.2
                                                ? `rgba(0, 255, 0, ${Math.min(1, Math.abs(1 -item.pcrOichange))})`
                                                : `rgba(128, 128, 128, ${Math.min(1, Math.abs(item.pcrOichange - 1))})`, // Adjusted for grey color
                                      
                                    }}
                                >
                                    {item.pcrOichange}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>

                {/* Table 2: Put OI, Call OI, Put OI Change, Call OI Change, PE VWAP, CE VWAP */}
                <table className="table" style={{ width: '48%', ...tableColumnStyle }}>
                    <thead>
                        <tr>
                           {/* <th>ID</th>*/}
                            <th>Time</th>
                            <th>Put OI</th>
                            <th>Call OI</th>
                            <th>Put OI Change</th>
                            <th>Call OI Change</th>
                            <th>PE VWAP</th>
                            <th>CE VWAP</th>
                            {/* Add other columns as needed */}
                        </tr>
                    </thead>
                    <tbody>
                        {oiData.map(item => (
                            <tr key={item.id}>
                                {/*<td>{item.id}</td>*/}
                                <td>{item.entryDateTime}</td>
                                <td>{item.putOi}</td>
                                <td>{item.callOi}</td>
                                <td>{item.putOichange}</td>
                                <td>{item.callOichange}</td>
                                <td>{item.pevwap}</td>
                                <td>{item.cevwap}</td>
                                {/* Add other columns as needed */}
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
                            <div>
                                <h2>Option Chain</h2>
                                <div>{getPredictionNotes()}</div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-12 col-md-4 offset-md-12">
                        {contents ?? (
                            <p>
                                <em>No Optionchain available</em>
                            </p>
                        )}

                        <div>{apistatus}</div>
                    </div>
                </div>
            </div>
        </section>
    );

    async function PopulateOIData() {
        const response = await fetch('api/nse', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });
        const data = await response.json();
        console.log('options chain data');
        console.log(data);
        if (response.ok) {
            // Registration successful
            if (data.status === 1) {
                // Format entryDateTime without seconds and round to nearest 5 minutes
                const formattedData = data.result.map(item => ({
                    ...item,
                    entryDateTime: roundToNearest5Minutes(item.entryDateTime),
                }));

                const sortedData = formattedData.sort((a, b) => b.id - a.id);
               
                setOiData(sortedData);
                setApiStatus(data.message);
            } else {
                setApiStatus('Error: ' + data.message);
                console.error('Error:', data.message);
                throw new Error(data.message);
            }
        } else {
            // Registration failed
            setApiStatus('oi retrieval failed!');
            console.error('oi retrieval failed!');
            throw new Error('oi retrieval failed!');
        }
    }
   
    function roundToNearest5Minutes(dateString) {
        const date = new Date(dateString);
        const roundedMinutes = Math.round(date.getMinutes() / 5) * 5;
        date.setMinutes(roundedMinutes);

        // Format the time without seconds
        const formattedTime = date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

        return formattedTime;
    }
};

export default OI;
