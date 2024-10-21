import React, { useState } from 'react';
import { TextField, Button, Box, Snackbar, Alert, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { login } from '../api/authApi';

const LoginForm: React.FC = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setLoading(true);
        setErrorMessage(null);
    
        if (!username || !password) {
            setErrorMessage('Please fill in both fields.'); 
            setLoading(false);
            return;
        }
    
        try {
            const response = await login(username, password);
            localStorage.setItem('authToken', response.data.token);
            localStorage.setItem('username', username); 
            navigate('/socialPost');
        } catch (error) {
            console.error('Login failed:', error);
            setErrorMessage('Invalid username or password.'); 
        } finally {
            setLoading(false); 
        }
    };

    const handleSnackbarClose = () => {
        setErrorMessage(null);
    };

    return (
        <Box 
            component="form" 
            onSubmit={handleSubmit} 
            sx={{ mt: 2, display: 'flex', flexDirection: 'column', alignItems: 'center' }}
        >
            <Typography variant="h5" component="h1" sx={{ mb: 2 }}>
                Login
            </Typography>
            
            <TextField
                margin="normal"
                required
                fullWidth
                label="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <TextField
                margin="normal"
                required
                fullWidth
                label="Password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            
            <Button type="submit" fullWidth variant="contained" sx={{ mt: 3, mb: 2 }} disabled={loading}>
                {loading ? 'Logging in...' : 'Login'}
            </Button>

            {/* Demo credentials info */}
            <Typography variant="body2" sx={{ mb: 2, color: 'gray' }}>
                Demo Username: <strong>admin</strong>, Password: <strong>123</strong>
            </Typography>

            {/* Snackbar for displaying error messages */}
            <Snackbar open={!!errorMessage} autoHideDuration={6000} onClose={handleSnackbarClose}>
                <Alert onClose={handleSnackbarClose} severity="error" sx={{ width: '100%' }}>
                    {errorMessage}
                </Alert>
            </Snackbar>
        </Box>
    );
};

export default LoginForm;
