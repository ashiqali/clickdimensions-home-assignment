import axios from 'axios';
import { API_URLS } from '../config/config';

export const login = async (username: string, password: string) => {
    return axios.post(API_URLS.auth.login, { username, password });
};

export const register = async (username: string, password: string, name: string, surname: string) => {
    return axios.post(API_URLS.auth.register, { 
        Username: username,
        Password: password,
        Name: name,
        Surname: surname
    });
};

export const refreshToken = async (token: string) => {
    return axios.post(API_URLS.auth.refresh, { token });
};
