import { jwtDecode } from 'jwt-decode';

export const checkTokenExpiration = () => {
    const user = JSON.parse(localStorage.getItem('userData'));

    if (user) {
        console.log(user);
        const decodedToken = jwtDecode(user.token);
        const expirationTime = decodedToken.exp * 1000; // Convert to milliseconds
        const isTokenExpired = Date.now() > expirationTime;

        if (!isTokenExpired) {
            // Token is still valid
            return { status: true, user: user };
        } else {
            // Token has expired
            console.log('Token has expired');
            return { status: false, user: null };
        }
    } else {
        // No token found
        console.log('No token found');
        return { status: false, user: null };
    }
};