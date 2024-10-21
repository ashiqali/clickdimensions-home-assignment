import React, { useState } from 'react';
import { TextField, Button, Box, Snackbar, Alert, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { register } from '../api/authApi';

const RegisterForm: React.FC = () => {
    const [username, setUsername] = useState('');
    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setLoading(true);
        setErrorMessage(null);
        setSuccessMessage(null);

        try {
            const response = await register(username, password, name, surname);
            localStorage.setItem('authToken', response.data.token);
            setSuccessMessage('Registration successful! Redirecting...');
            setTimeout(() => navigate('/socialPost'), 2000);
        } catch (error) {
            console.error('Registration failed:', error);
            setErrorMessage('Registration failed. Please try again.');
        } finally {
            setLoading(false);
        }
    };

    const handleSnackbarClose = () => {
        setErrorMessage(null);
        setSuccessMessage(null);
    };

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            <Typography variant="h5" component="h1" sx={{ mb: 2 }}>
                Create an Account
            </Typography>
            <TextField
                margin="normal"
                required
                fullWidth
                label="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                sx={{ mb: 2 }}
            />
            <TextField
                margin="normal"
                required
                fullWidth
                label="Name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                sx={{ mb: 2 }}
            />
            <TextField
                margin="normal"
                required
                fullWidth
                label="Surname"
                value={surname}
                onChange={(e) => setSurname(e.target.value)}
                sx={{ mb: 2 }}
            />
            <TextField
                margin="normal"
                required
                fullWidth
                label="Password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                sx={{ mb: 2 }}
            />
            <Button type="submit" fullWidth variant="contained" sx={{ mt: 2 }} disabled={loading}>
                {loading ? 'Registering...' : 'Register'}
            </Button>

            {/* Snackbar for displaying error or success messages */}
            <Snackbar open={!!errorMessage} autoHideDuration={6000} onClose={handleSnackbarClose}>
                <Alert onClose={handleSnackbarClose} severity="error" sx={{ width: '100%' }}>
                    {errorMessage}
                </Alert>
            </Snackbar>

            <Snackbar open={!!successMessage} autoHideDuration={6000} onClose={handleSnackbarClose}>
                <Alert onClose={handleSnackbarClose} severity="success" sx={{ width: '100%' }}>
                    {successMessage}
                </Alert>
            </Snackbar>
        </Box>
    );
};

export default RegisterForm;
