import React, { useState, useEffect } from 'react';
import {
    TextField,
    Button,
    Snackbar,
    Alert,
    Select,
    MenuItem,
    SelectChangeEvent,
} from '@mui/material';
import { SocialPostDTO } from '../types/socialPost';
import { createSocialPost, getSocialPostById, updateSocialPost } from '../api/socialPostApi';
import axios from 'axios';

interface SocialPostFormProps {
    postId: number | null;
    onSuccess: () => void;
}

const SocialPostForm: React.FC<SocialPostFormProps> = ({ postId, onSuccess }) => {
    const [socialPost, setSocialPost] = useState<SocialPostDTO>({
        id: 0,
        title: '',
        author: '',
        source: '',
        publishedDate: '',
        popularity: 0,
    });
    const [errorMessage, setErrorMessage] = useState<string | null>(null); 
    const [loading, setLoading] = useState<boolean>(false); 

    useEffect(() => {
        if (postId) {
            loadSocialPost();
        }
    }, [postId]);

    const loadSocialPost = async () => {
        try {
            const response = await getSocialPostById(postId!);
            setSocialPost(response.data);
            setErrorMessage(null); 
        } catch (error) {
            setErrorMessage('Error loading SocialPost details.'); 
            console.error('Error loading SocialPost:', error);
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setSocialPost({ ...socialPost, [name]: value });
    };

    const handleSelectChange = (event: SelectChangeEvent<string>) => {
        const { name, value } = event.target;
        setSocialPost({ ...socialPost, [name]: value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true); 
        try {
            if (postId) {
                await updateSocialPost(postId, socialPost);
            } else {
                await createSocialPost(socialPost);
            }
            onSuccess();
            setErrorMessage(null); 
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                setErrorMessage(`Error submitting SocialPost details: ${error.response.data}`);
            } else {
                setErrorMessage('Error submitting SocialPost details. Please try again later.');
            }
            console.error('Error submitting SocialPost details:', error);
        } finally {
            setLoading(false); 
        }
    };

    const handleSnackbarClose = () => {
        setErrorMessage(null); 
    };

    return (
        <>
            <form onSubmit={handleSubmit}>
                <TextField
                    name="title"
                    label="Title"
                    value={socialPost.title}
                    onChange={handleChange}
                    required
                    fullWidth
                    margin="normal"
                />
                <TextField
                    name="author"
                    label="Author"
                    value={socialPost.author}
                    onChange={handleChange}
                    required
                    fullWidth
                    margin="normal"
                />
                <Select
                    name="source"
                    value={socialPost.source}
                    onChange={handleSelectChange}
                    required
                    fullWidth
                    displayEmpty
                >
                    <MenuItem value="">
                        <em>Select Source</em>
                    </MenuItem>
                    <MenuItem value="Reddit">Reddit</MenuItem>
                    <MenuItem value="Tumblr">Tumblr</MenuItem>
                </Select>
                <TextField
                    name="popularity"
                    label="Popularity"
                    value={socialPost.popularity}
                    onChange={handleChange}
                    fullWidth
                    margin="normal"
                />
                <Button type="submit" variant="contained" color="primary" style={{ marginTop: '16px' }} disabled={loading}>
                     {postId ? 'Update Post' : 'Add Post'}
                </Button>
            </form>

            {/* Snackbar for displaying error messages */}
            <Snackbar open={!!errorMessage} autoHideDuration={6000} onClose={handleSnackbarClose}>
                <Alert onClose={handleSnackbarClose} severity="error" sx={{ width: '100%' }}>
                    {errorMessage}
                </Alert>
            </Snackbar>
        </>
    );
};

export default SocialPostForm;
