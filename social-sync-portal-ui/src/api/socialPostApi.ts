import axios from 'axios';
import { refreshToken } from './authApi';
import { API_URLS } from '../config/config';

const apiClient = axios.create();

// Request interceptor to include token
apiClient.interceptors.request.use((config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Response interceptor to refresh token if necessary
apiClient.interceptors.response.use(
    response => response,
    async error => {
        const originalRequest = error.config;

        // If 401 error and the request has not already been retried
        if (error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            try {
                const token = localStorage.getItem('authToken');
                const response = await refreshToken(token!);
                localStorage.setItem('authToken', response.data.token);
                axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
                return apiClient(originalRequest); 
            } catch (err) {
                console.error("Token refresh failed:", err);
            }
        }
        return Promise.reject(error);
    }
);

export const getAllSocialPosts = async () => apiClient.get(API_URLS.socialPost.base);

export const getSocialPostById = async (id: number) => apiClient.get(`${API_URLS.socialPost.base}/${id}`);


export const createSocialPost = async (socialPost: any) => {
    // Create a copy of the socialPost object without the 'id' and 'publishedDate' field
    const { id,publishedDate, ...socialPostWithoutDate } = socialPost;

    // Send the modified object to the backend
    return apiClient.post(API_URLS.socialPost.base, socialPostWithoutDate);
};

export const updateSocialPost = async (id: number, socialPost: any) => apiClient.put(`${API_URLS.socialPost.base}/${id}`, socialPost);

export const deleteSocialPost = async (id: number) => apiClient.delete(`${API_URLS.socialPost.base}/${id}`);
