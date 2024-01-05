import React, { useState } from 'react';
import ShoonyaUserGuide from './Brokers/ShoonyaUserGuide'
import UserGuidesMenu from './UserGuidesMenu';
function UserGuides() {
    const [selectedComponent, setSelectedComponent] = useState('shoonyauserguide');

    const handleNavLinkClick = (page) => {
        setSelectedComponent(page);
    };

    const renderSelectedComponent = () => {
        switch (selectedComponent) {
            case 'shoonyauserguide':
                return <ShoonyaUserGuide />;

            default:
                return <ShoonyaUserGuide />;
        }
    };

    return (
        <section className="contact_section layout_padding-top">
            <div className="container-fluid">
                <UserGuidesMenu onNavLinkClick={handleNavLinkClick} />
                {/*<div className="content">*/}
                    {renderSelectedComponent()}
           {/*     </div>*/}
            </div>
        </section>
    );
}

export default UserGuides;