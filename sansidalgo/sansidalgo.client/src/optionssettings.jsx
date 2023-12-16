import React from 'react'
import { useState, useEffect } from 'react';
function OptionsSettings({ onFormChange, initialValues }) {


    const [formData, setFormData] = useState({
      
        instrument: '',
        expiryDay: '',
        lotSize: 0,
        ceSideEntryAt: 0,
        peSideEntryAt: 0,
      

    });
 
    useEffect(() => {
  
        setFormData(initialValues)
    }, [initialValues]);
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
        onFormChange(formData);
    };

    return (
        <div className="boxTypeDiv">
            <div>
                <label>Name:</label>
                <input name="name" value={formData.name || ''} onChange={handleChange} type="text" placeholder="Enter the name" />
            </div>
            <div>
                <label >Instrument:</label>
                <input name="instrument" value={formData.instrument || ''} onChange={handleChange} type="text" placeholder="Enter the instrument" />
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
                <input name="peSideEntryAt" value={formData.peSideEntryAt || ''} onChange={handleChange} type="text" placeholder="Enter the PE side entry at" />
            </div>
        </div>
    );

}
export default OptionsSettings;