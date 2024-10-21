import React from 'react';
import { Container } from '@mui/material';
import RegisterForm from '../components/RegisterForm';

const RegisterPage: React.FC = () => {
    return (
        <Container component="main" maxWidth="xs">            
            <RegisterForm />
        </Container>
    );
};

export default RegisterPage;
