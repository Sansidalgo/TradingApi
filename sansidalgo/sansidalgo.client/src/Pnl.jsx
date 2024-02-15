import React, { useState } from 'react';
import moment from 'moment';
import { DatePicker } from 'antd';
//import 'antd/dist/antd.css';

const Pnl = () => {
    // Sample profit or loss data (replace it with your actual data)
    const [profitLossData, setProfitLossData] = useState([
        { date: '2024-02-01', value: 100 },
        { date: '2024-02-02', value: -50 },
        { date: '2024-02-03', value: -150 },
        { date: '2024-02-04', value: 250 },
        { date: '2024-02-05', value: 350 },
        { date: '2024-02-06', value: 50 },
        { date: '2024-02-07', value: -50 },
        { date: '2024-02-08', value: 150 },
        { date: '2024-02-09', value: 100 },
        { date: '2024-02-10', value: -150 },
        { date: '2024-02-11', value: 250 },
        { date: '2024-02-12', value: 200 },
        { date: '2024-02-13', value: -100 },
        { date: '2024-02-14', value: -50 },
        { date: '2024-02-15', value: 150 },
        { date: '2024-02-16', value: 250 },
        { date: '2024-02-17', value: 350 },
        { date: '2024-02-18', value: -50 },
        { date: '2024-02-20', value: -150 },
        { date: '2024-02-21', value: 550 },
        { date: '2024-02-22', value: 50 },
        // Add more data for other days
    ]);

    // Calculate total value
    const totalValue = profitLossData.reduce((total, entry) => total + entry.value, 0);
    //const defaultEndDate = moment(); // Current date
    //const defaultStartDate = moment().subtract(1, 'month');
    const [dateRange, setDateRange] = useState([null, null]); //useState([defaultStartDate, defaultEndDate]);
    const { RangePicker } = DatePicker;

    const handleDateChange = (dates) => {
        setDateRange(dates);
    };

    // Calculate the difference in days between the target date and the current date
    const currentDate = moment(profitLossData[0].date);

    // Get the weekday number (1 for Sunday, 2 for Monday, etc.)
    const currentWeekdayNumber = currentDate.isoWeekday();
    const daysDifference = currentWeekdayNumber;
        // Render empty divs and the target date div
    
    const additionalData = [];
    for (let i = 1; i <= daysDifference; i++) {
        additionalData.push({
            date: currentDate.clone().subtract(i, 'day').format('YYYY-MM-DD'),
            value: ""
        });
    }
      
    return (
        <section className="contact_section layout_padding-top">
        <div className="row">
                <div className="col-lg-4 col-md-5 offset-md-1">
                    <div className="heading_container">
                        <h2>Profit And Loss</h2>
                        <br></br>
                        <div><b> Choose Date Range</b>  &nbsp;
                            <RangePicker value={dateRange} onChange={handleDateChange} />
                        </div>
                    </div>
                </div>
            </div>
            
            
            
            <div className="calendar-grid">
                {
                    additionalData.map((day) => (
                        <div key={day.date} className={`grid-box holiday`}>
                        </div>
                    ))
                }
                {
                    profitLossData.map((day) => (
                <div key={day.date} className={`grid-box ${day.value >= 0 ? 'profit' : 'loss'}`}>
                    
                    <b>
                        <span className="value">{day.value >= 0 ? `+${day.value}` : day.value}</span>
                    </b>
                    <br />
                    <span className="date">{moment(day.date).format('MMM DD')}</span>
                </div>
            ))}
                <b><div>Total PNL : {totalValue}</div></b>
            </div>
        </section>
    );
};

export default Pnl;