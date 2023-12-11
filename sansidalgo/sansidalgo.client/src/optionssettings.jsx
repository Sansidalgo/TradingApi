import React from 'react'
import { useState } from 'react';
function optionssettings({ onFormChange }) {


    const [formData, setFormData] = useState({
        Name:'',
        Instrument: '',
        ExpiryDay: '',
        LotSize: 0,
        CeSideEntryAt: 0,
        PeSideEntryAt: 0

    });
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
        onFormChange(formData);
    };

    return (

        <div className="boxTypeDiv">
            <div>
                <input name="Name" value={formData.Name} onChange={handleChange} type="text" placeholder="Name" />
            </div>
            <div>
                <input name="Instrument" value={formData.Instrument} onChange={handleChange} type="text" placeholder="Instrument" />
            </div>
            <div>
                <input name="ExpiryDay" value={formData.ExpiryDay} onChange={handleChange} type="text" placeholder="ExpiryDay" />
            </div>
            <div>
                <input name="LotSize" value={formData.LotSize} onChange={handleChange} type="text" placeholder="LotSize" />
            </div>
            <div>
                <input name="CeSideEntryAt" value={formData.CeSideEntryAt} onChange={handleChange} type="text" placeholder="CeSideEntryAt" />
            </div>
            <div>
                <input name="PeSideEntryAt" value={formData.PeSideEntryAt} onChange={handleChange} type="text" placeholder="PeSideEntryAt" />
            </div>
           
            
        </div>

    );
}
export default optionssettings;