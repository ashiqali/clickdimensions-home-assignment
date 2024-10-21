import React, { useState, useEffect } from 'react';
import {
    Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, IconButton, TablePagination,
    Dialog, DialogTitle, DialogContent, DialogActions, Button, Snackbar, Alert, TextField,Tooltip, InputAdornment
} from '@mui/material';
import { getAllSocialPosts, deleteSocialPost } from '../api/socialPostApi';
import { SocialPostDTO } from '../types/socialPost';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import AddIcon from '@mui/icons-material/Add';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';
import ExportIcon from '@mui/icons-material/SaveAlt';
import SocialPostForm from './SocialPostForm';
import { format } from 'date-fns';
import * as XLSX from 'xlsx';

const SocialPostList: React.FC = () => {
    const [socialPost, setSocialPost] = useState<SocialPostDTO[]>([]);
    const [filteredSocialPost, setFilteredSocialPost] = useState<SocialPostDTO[]>([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [openDialog, setOpenDialog] = useState(false);
    const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
    const [socialPostToDelete, setSocialPostToDelete] = useState<number | null>(null);
    const [editingPostId, setEditingPostId] = useState<number | null>(null);
    const [errorMessage, setErrorMessage] = useState<string | null>(null); 
    const [successMessage, setSuccessMessage] = useState<string | null>(null); 

    useEffect(() => {
        loadSocialPost();
    }, []);

    useEffect(() => {
        // Filter SocialPost based on the search term
        setFilteredSocialPost(
            socialPost.filter(post =>
                post.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
                post.author.toLowerCase().includes(searchTerm.toLowerCase()) ||
                post.source.toLowerCase().includes(searchTerm.toLowerCase()) ||
                format(new Date(post.publishedDate), 'dd/MM/yyyy').includes(searchTerm.toLowerCase())
            )
        );
    }, [searchTerm, socialPost]);

    const loadSocialPost = async () => {
        try {
            const response = await getAllSocialPosts();
            setSocialPost(response.data);
            setFilteredSocialPost(response.data); 
            setErrorMessage(null); 
        } catch (error) {
            setErrorMessage('Error loading SocialPost. Please try again later.');
            console.error('Error loading SocialPost:', error);
        }
    };

    const handleDelete = async () => {
        if (socialPostToDelete !== null) {
            try {
                await deleteSocialPost(socialPostToDelete);
                loadSocialPost(); 
                handleCloseDeleteDialog(); 
                setSuccessMessage('SocialPost deleted successfully');
                setErrorMessage(null); 
            } catch (error) {
                setErrorMessage('Error deleting post. Please try again later.'); 
                console.error('Error deleting post:', error);
            }
        }
    };

    const handleChangePage = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const handleEditClick = (id: number) => {
        setEditingPostId(id);
        setOpenDialog(true);
    };

    const handleAddClick = () => {
        setEditingPostId(null);
        setOpenDialog(true);
    };

    const handleDialogClose = () => {
        setOpenDialog(false);
        setEditingPostId(null);
    };

    const handleFormSuccess = (message: string) => {
        handleDialogClose();
        loadSocialPost(); 
        setSuccessMessage(message);
    };

    const handleOpenDeleteDialog = (id: number) => {
        setSocialPostToDelete(id);
        setOpenDeleteDialog(true);
    };

    const handleCloseDeleteDialog = () => {
        setOpenDeleteDialog(false);
        setSocialPostToDelete(null);
    };

    const handleSnackbarClose = () => {
        setErrorMessage(null);
        setSuccessMessage(null);
    };

    const handleClearSearch = () => {
        setSearchTerm(''); 
    };

    const handleExportToExcel = () => {
        const worksheet = XLSX.utils.json_to_sheet(filteredSocialPost);
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, 'SocialPost');
        
        XLSX.writeFile(workbook, 'SocialPost.xlsx');
    };

    return (
        <Paper>            
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '10px' }}>
                <TextField
                    label="Search"
                    variant="outlined"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)} 
                    style={{ marginRight: '10px', flexGrow: 1 }} 
                    InputProps={{
                        startAdornment: (
                            <InputAdornment position="start">
                                <SearchIcon /> 
                            </InputAdornment>
                        ),
                        endAdornment: (
                            searchTerm && (
                                <InputAdornment position="end">
                                    <IconButton onClick={handleClearSearch}>
                                        <ClearIcon /> 
                                    </IconButton>
                                </InputAdornment>
                            )
                        )
                    }}
                />
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={handleAddClick}
                >
                    Add Social Post
                </Button>
                <Button
                    variant="contained"
                    color="secondary"
                    onClick={handleExportToExcel} 
                    startIcon={<ExportIcon />}
                    style={{ marginLeft: '10px' }}
                >
                    Export to Excel
                </Button>
            </div>

            <TableContainer>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Title</TableCell>
                            <TableCell>Author</TableCell>
                            <TableCell>Source</TableCell>
                            <TableCell>PublishedDate</TableCell>
                            <TableCell>Popularity</TableCell>
                            <TableCell align="right">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {filteredSocialPost.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((post) => (
                            <TableRow key={post.id}>
                                 <TableCell>
                                    <Tooltip title={post.title}>
                                    <span>{post.title.length > 35 ? `${post.title.substring(0, 35)}...` : post.title}</span>
                                    </Tooltip>
                                </TableCell>
                                <TableCell>{post.author}</TableCell>
                                <TableCell>{post.source}</TableCell>
                                <TableCell>{format(new Date(post.publishedDate), 'dd/MM/yyyy')}</TableCell>
                                <TableCell>
                                    {post.popularity}
                                </TableCell>
                                <TableCell align="right">
                                    <IconButton edge="end" aria-label="edit" onClick={() => handleEditClick(post.id!)} >
                                        <EditIcon />
                                    </IconButton>
                                    <IconButton edge="end" aria-label="delete" onClick={() => handleOpenDeleteDialog(post.id!)} >
                                        <DeleteIcon />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            {/* Display total record count */}
            <div style={{ padding: '10px' }}>
                Total Records: {filteredSocialPost.length}
            </div>

            <TablePagination
                component="div"
                count={filteredSocialPost.length}
                page={page}
                onPageChange={handleChangePage}
                rowsPerPage={rowsPerPage}
                onRowsPerPageChange={handleChangeRowsPerPage}
                rowsPerPageOptions={[5, 10, 25]}
                labelRowsPerPage="Rows per page"
                showFirstButton
                showLastButton
            />

            {/* Dialog for adding/editing SocialPost */}
            <Dialog open={openDialog} onClose={handleDialogClose} maxWidth="md" fullWidth>
                <DialogTitle>{editingPostId ? "Edit Social Post" : "Add Social Post"}</DialogTitle>
                <DialogContent>
                    <SocialPostForm postId={editingPostId} onSuccess={() => handleFormSuccess(editingPostId ? 'Post updated successfully' : 'Post added successfully')} />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleDialogClose} color="primary">
                        Cancel
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Confirmation Dialog for deleting a SocialPost */}
            <Dialog open={openDeleteDialog} onClose={handleCloseDeleteDialog} maxWidth="sm" fullWidth>
                <DialogTitle>Delete Social Post</DialogTitle>
                <DialogContent>
                    Are you sure you want to delete this post?
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseDeleteDialog} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={handleDelete} color="secondary">
                        Confirm Delete
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Snackbar for displaying success and error messages */}
            <Snackbar open={!!errorMessage || !!successMessage} autoHideDuration={6000} onClose={handleSnackbarClose}>
                {errorMessage ? (
                    <Alert onClose={handleSnackbarClose} severity="error" sx={{ width: '100%' }}>
                        {errorMessage}
                    </Alert>
                ) : (
                    <Alert onClose={handleSnackbarClose} severity="success" sx={{ width: '100%' }}>
                        {successMessage}
                    </Alert>
                )}
            </Snackbar>
        </Paper>
    );
};

export default SocialPostList;
