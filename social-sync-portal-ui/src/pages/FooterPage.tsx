import React from 'react';
import { AppBar, Toolbar, Typography } from '@mui/material';

const FooterPage: React.FC = () => {
    return (
        <AppBar position="static" style={{ bottom: 0 }}>
            <Toolbar>
                <Typography variant="body1" style={{ flexGrow: 1, textAlign: 'center' }}>
                    © 2024 SocialSync Portal Management. All rights reserved.
                </Typography>
            </Toolbar>
        </AppBar>
    );
};

export default FooterPage;
