import React from 'react';
import { AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Link, useNavigate ,useLocation} from 'react-router-dom';

const HeaderPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const handleLogout = () => {
        localStorage.removeItem('authToken');
        localStorage.removeItem('username'); 
        navigate('/login'); 
    };

    const isAuthenticated = () => {
        return !!localStorage.getItem('authToken');
    };

    const getUsername = () => {
        return localStorage.getItem('username');
    };

    return (
        <AppBar position="static">
            <Toolbar>
                <Typography variant="h6" style={{ flexGrow: 1 }}>
                    Social Sync Management Portal
                </Typography>
                {isAuthenticated() ? (
                    <>
                        <Typography variant="body1" style={{ marginRight: '16px' }}>
                            Welcome, {getUsername()}!
                        </Typography>
                        <Button color="inherit" onClick={handleLogout}>
                            Logout
                        </Button>
                    </>
                ) : (
                    <>
                     {location.pathname === '/register' && (
                            <Button color="inherit" component={Link} to="/login">
                                Login
                            </Button>
                        )}

                    {location.pathname === '/login' && (
                            <Button color="inherit" component={Link} to="/register">
                            Register
                        </Button>
                        )}
                        
                    </>
                )}
            </Toolbar>
        </AppBar>
    );
};

export default HeaderPage;
