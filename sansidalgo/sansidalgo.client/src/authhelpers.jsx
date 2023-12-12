import { jwtDecode } from 'jwt-decode';

export const checkTokenExpiration = () => {
    const user = JSON.parse(localStorage.getItem('userData'));

    if (user) {
        console.log(user);
        const decodedToken = jwtDecode(user.token); // Update this line
        const expirationTime = decodedToken.exp * 1000; // Convert to milliseconds
        const isTokenExpired = Date.now() > expirationTime;

        if (!isTokenExpired) {
            // Token is still valid
            const res = { status: true, user: user };
            console.log(res);
            return res;
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
