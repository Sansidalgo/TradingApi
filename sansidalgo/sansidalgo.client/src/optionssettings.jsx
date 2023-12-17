import React from 'react'
import { useState, useEffect } from 'react';
import Dropdown from './DropDown'
function OptionsSettings({ onFormChange, initialValues }) {


    const [formData, setFormData] = useState({
        id:0,
        instrumentId: 0,
        expiryDay: '',
        ceSideEntryAt: 0,
        peSideEntryAt: 0,
        strategyId: 0,
        exchange:'',
        lotSize: 0,
        name: '',
        startTime: '',
        endTime: '',
        playCapital: 0,
        playQuantity: 0,
        stopLoss: 0,
        target: 0,
        trailingStopLoss: 0,
        trailingTarget:0
       
      

    });
    const [apistatus, setApiStatus] = useState("");
    const handleInstrumentApiStatusChange = (newApiStatus) => {
        setApiStatus(newApiStatus);
    };
    const handleInstrumentSelectedOptionChange = (selectedOption) => {
        setFormData((prevFormData) => ({
            ...prevFormData,
            instrumentId: selectedOption
        }));
       
      
    };

    useEffect(() => {
  
        setFormData(initialValues)
    }, [initialValues]);

    useEffect(() => {

        onFormChange(formData);
    }, [formData]);


   

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
   
    };

    return (
        <div className="boxTypeDiv">
            <div>
                <div style={{ display: 'flex', flexDirection: 'row' }}>
                    <label >Name:</label>
                    <div style={{ color: 'red' }}> *</div>
                </div>
                <input name="name" value={formData.name || ''} onChange={handleChange} type="text" placeholder="Enter the name" />
                <input name="id" value={formData.id} onChange={handleChange} type="hidden" />
            </div>
            <div>
                
                <div style={{ display: 'flex', flexDirection: 'row' }}>
                    <label >Instrument Id:</label>
                    <div style={{ color: 'red' }}> *</div>
                </div>
                <Dropdown
                    apiPath="/api/Instruments/GetInstruments"
                    placeholder="Select Instrument"
                    displayProperty="name"
                    valueProperty="id"
                    onApiStatusChange={handleInstrumentApiStatusChange}
                    onSelectedOptionChange={handleInstrumentSelectedOptionChange}
                    selectedItemId={formData.instrumentId}

                />
               
            </div>
            <div>
                <label >Expiry Day:</label>
                <input name="expiryDay" value={formData.expiryDay || ''} onChange={handleChange} type="text" placeholder="Enter the expiry day" />
            </div>
            <div>
                <label >Lot Size:</label>
                <input name="lotSize" value={formData.lotSize || ''} onChange={handleChange} type="text" placeholder="Enter the lot size" />
            </div>
            <div>
                <label >CE Side Entry At:</label>
                <input name="ceSideEntryAt" value={formData.ceSideEntryAt || ''} onChange={handleChange} type="text" placeholder="Enter the CE side entry at" />
            </div>
            <div>
                <label >PE Side Entry At:</label>
                <input
                    name="peSideEntryAt" value={formData.peSideEntryAt || ''} onChange={handleChange} type="text" placeholder="Enter the PE side entry at" />
            </div>
            <div>
                <label >Stretegy:</label>
                <input name="strategyId" value={formData.strategyId || ''} onChange={handleChange} type="text" placeholder="Enter the strategyId" />
            </div>
            <div>
                <label >Exchange:</label>
                <input name="exchange" value={formData.exchange || ''} onChange={handleChange} type="text" placeholder="Enter theexchange" />
            </div>

           

            <div>
                <label >Start Time:</label>
                <input name="startTime" value={formData.startTime || ''} onChange={handleChange} type="text" placeholder="Enter the start time" />
            </div>
            <div>
                <label >End Time:</label>
                <input name="endTime" value={formData.endTime || ''} onChange={handleChange} type="text" placeholder="Enter the end time" />
            </div>
            <div>
                <label >Play Capital:</label>
                <input name="playCapital" value={formData.playCapital || ''} onChange={handleChange} type="text" placeholder="Enter the play capital" />
            </div>
            <div>
                <label >Play Quantity:</label>
                <input name="playQuantity" value={formData.playQuantity || ''} onChange={handleChange} type="text" placeholder="Enter the Play Quantity" />
            </div>
            <div>
                <label >Stop Loss:</label>
                <input name="stopLoss" value={formData.stopLoss || ''} onChange={handleChange} type="text" placeholder="Enter the Stop Loss" />
            </div>
            <div>
                <label >Target:</label>
                <input name="target" value={formData.target || ''} onChange={handleChange} type="text" placeholder="Enter the target" />
            </div>
            <div>
                <label >Trailing Stop Loss:</label>
                <input name="trailingStopLoss" value={formData.trailingStopLoss || ''} onChange={handleChange} type="text" placeholder="Enter the Trailing Stop Loss" />
            </div>

            <div>
                <label >Trailing Target:</label>
                <input name="trailingTarget" value={formData.trailingTarget || ''} onChange={handleChange} type="text" placeholder="Enter the Trailing Target" />
            </div>


        </div>
    );

}
export default OptionsSettings;