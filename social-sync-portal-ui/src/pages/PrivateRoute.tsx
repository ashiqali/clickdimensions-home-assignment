import React from 'react';
import { Navigate } from 'react-router-dom';

interface PrivateRouteProps {
    children: React.ReactNode;
    redirectPath?: string;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ children, redirectPath = '/login' }) => {
    const token = localStorage.getItem('authToken');
        
    return token ? <>{children}</> : <Navigate to={redirectPath} />;
};

export default PrivateRoute;
