// src/components/LeftNav.js
import React from 'react';


const UserGuidesMenu = ({ onNavLinkClick }) => {
    return (
        <div className="leftnav">
            <a className="active" href="#" onClick={() => onNavLinkClick('shoonyauserguide')}>Shoonya</a>
            <a href="#">Fyers</a>
            <a href="#" >Zerodha</a>
        </div>
    );
};

export default UserGuidesMenu;
