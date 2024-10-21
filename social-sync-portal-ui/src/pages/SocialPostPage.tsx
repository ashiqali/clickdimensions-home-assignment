import React from 'react';
import { useNavigate } from 'react-router-dom';
import SocialPostList from '../components/SocialPostList';

const SocialPostPage: React.FC = () => {
    const navigate = useNavigate();
    
    return (
        <div>                        
            <SocialPostList />
        </div>
    );
};

export default SocialPostPage;
