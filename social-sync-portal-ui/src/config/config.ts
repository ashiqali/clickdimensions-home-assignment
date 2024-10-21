//const BASE_URL = 'http://localhost:5001/api/v1'; //http

const BASE_URL = 'https://localhost:5000/api/v1'; //https

export const API_URLS = {
    auth: {
        login: `${BASE_URL}/auth/login`,
        register: `${BASE_URL}/auth/register`,
        refresh: `${BASE_URL}/auth/refresh`,
    },
    socialPost: {
        base: `${BASE_URL}/socialPost`,
    },
};
