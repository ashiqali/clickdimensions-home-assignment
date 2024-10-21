import React from 'react';
import { Container } from '@mui/material';
import HeaderPage from '../pages/HeaderPage';
import FooterPage from '../pages/FooterPage';

const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    return (
        <div style={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
            <HeaderPage />
            <Container component="main" style={{ flex: 1, padding: '20px' }}>
                {children}
            </Container>
            <FooterPage />
        </div>
    );
};

export default MainLayout;
