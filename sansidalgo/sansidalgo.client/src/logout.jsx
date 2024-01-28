import React from 'react';

const Logout = ({ handleLogout }) => {
    // Call the handleLogout function when this component mounts
    React.useEffect(() => {
        handleLogout();
        localStorage.removeItem('token');
    }, [handleLogout]);

    return (
        <div>
            <p>Logging out...</p>
        </div>
    );
};

export default Logout;