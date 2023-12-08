import React from 'react';

const logout = ({ handleLogout }) => {
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

export default logout;