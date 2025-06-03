import React from 'react';
import { useState, useEffect, useRef } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import userManager from '../../AuthFiles/authConfig';
import { NavLink } from 'react-router-dom';
import { ThreeDots } from 'react-loader-spinner';
import { useAuth } from './../AuthProvider';
import config from '../../config.json';
import { ToastContainer, toast } from 'react-toastify';
import './Profile.css'
import OrderHistory from './OrderHistory/OrderHistory.jsx'

const Profile = () => {
    const [isHovered, setIsHovered] = useState(false);
    const { user, userData, loading, isAuthorized, setLoadingState,
        setIsAuthorizedState,
        setUserState,
        setUserDataState } = useAuth();

    const [isNoPersonWarning, setIsNoPersonWarning] = useState(false);

    useEffect(() => {
        if (isAuthorized) {
            try {
                setName(userData.name);
                setPreferences(userData.preferences);
                setDateOfBirth(new Date(userData.dateOfBirth).toISOString().slice(0, 10));
            }
            catch {
                console.log("Excheption while setting user info");
            }
        }

    }, [isAuthorized]);
    const [name, setName] = useState(null);
    const [preferences, setPreferences] = useState(null);
    const [dateOfBirth, setDateOfBirth] = useState(null); 


    const handleSave = async () => {
        setLoadingState(true); 
        const updatedUserData = {
            name,
            preferences,
            dateOfBirth
        };

        try {
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/userUpdate`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedUserData)
            });

            if (response.ok) {
                setName(updatedUserData.name);
                setPreferences(updatedUserData.preferences);
                setDateOfBirth(updatedUserData.dateOfBirth);
            } else {
                throw new Error('Failed to update profile');
            }
        } catch (error) {
            console.error('Error updating profile:', error);
            alert('An error occurred while updating the profile.');
        } finally {
            setLoadingState(false);
        }
    };


    const handleMouseEnter = () => {
        setIsHovered(true);
    }

    const handleMouseLeave = () => {
        setIsHovered(false);
    }

    const handleImageUpload = (e) => {
        setLoadingState(true);
        const file = e.target.files[0];
        const maxSize = 5 * 1024 * 1024;
        const maxResolution = 1920;
        setIsNoPersonWarning(false);
        if (file && !isImageFile(file)) {
            setLoadingState(false);
            return;
        }

        if (file && file.size > maxSize) {
           
            setLoadingState(false);
            return;
        }

        if (file) {
            const img = new Image();
            img.onload = () => {
                const width = img.width;
                const height = img.height;
                if (width > maxResolution || height > maxResolution) {
                   console.log("Too huge photo");
                    setLoadingState(false);
                    return;
                }
                else {
                    const reader = new FileReader();
                    reader.onloadend = async () => {
                        const imageData = reader.result;
                        const blob = new Blob([new Uint8Array(imageData)], { type: file.type });
                        const base64Avatar = await new Promise((resolve) => {
                            const reader = new FileReader();
                            reader.onloadend = () => resolve(reader.result.split(',')[1]);
                            reader.readAsDataURL(blob);

                        });
                        try {
                            const accessToken = await userManager.getUser().then(user => user.access_token);
                            const response = await fetch(`${config.apiBaseUrl}/userProfilePhotoUpload`, {
                                method: 'POST',
                                headers: {
                                    'Authorization': `Bearer ${accessToken}`,
                                    'Content-Type': 'application/json'
                                },
                                body: JSON.stringify({ avatar: base64Avatar })
                            });
                            if (!response.ok && response.status === 400) {
                                setIsNoPersonWarning(true);
                            } else {
                                const data = await response.json();
                                setUserDataState(data);
                                setIsNoPersonWarning(false); 
                            }


                        } catch (err) {
                           
                            console.error('Error while sending the request', err);
                        } finally {
                            setLoadingState(false);
                        }
                    };
                    reader.readAsArrayBuffer(file);
                }
            };
            img.src = URL.createObjectURL(file);
        }
    };

    const handleImageDelete = async (e) => {
        setLoadingState(true);
        if (userData.photo == null || (Array.isArray(userData.photo) && userData.photo.length === 0) || userData.photo == '') {
            toast.error('You do not have a photo', {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
            });
            e.target.value = null;

            setLoadingState(false);
            return;
        }

        try {

            const accessToken = await userManager.getUser().then(user => user.access_token);
            await fetch(`${config.apiBaseUrl}/userProfilePhotoUpload`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ avatar: "" })
            });
            userData.photo = null;
            toast.success('Profile photo was deleted successfully.', {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
            });
        }
        catch (err) {
            toast.error(err, {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
            });
        }
        setLoadingState(false);
    };

    const isImageFile = (file) => {
        const acceptedImageTypes = ['image/jpeg', 'image/png', 'image/svg+xml', 'image/webp'];

        return acceptedImageTypes.includes(file.type);
    };


    return (
        <div className="ProfileMain">
            {loading ? (
                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <ThreeDots color="orange" height={80} width={80} />
                </div>
            ) : isAuthorized === false ? (
                <div>UnAuthorized</div>
            ) : (

                <div className="ProfileDiv">
                    <div className="Left_ProfileDiv">
                                <div className="avatar-container" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave}>
                                    <img src={userData.photo ? `data:image/jpeg;base64,${userData.photo}` : "/NoPhoto.jpg"} alt="Avatar" className="avatar" />
                                    <div className="buttons-container">
                                        <label className="edit-button">
                                            New
                                            <input type="file" name="clientAvatar" accept="image/*" onChange={handleImageUpload} style={{ display: 'none' }} capture="false" />
                                        </label>

                                        <label className="edit-button" onClick={handleImageDelete}>
                                            Delete
                                        </label>
                                        
                                    </div>

                                </div>
                                {isNoPersonWarning && (
                                    <div className="no-person-warning" style={{ color: 'red', marginTop: '10px' }}>
                                        The uploaded photo must contain a person.
                                    </div>
                                )}
                    </div>
                    <div className="Right_ProfileDiv">
                                <div className="Right_ProfileDiv_Title">
                                    <input
                                        type="text"
                                        value={name}
                                        onChange={(e) => setName(e.target.value)}
                                        className="input-title"
                                    />
                                </div>
                                <div className="ProfileBio">
                                    <div className="ProfileBio_Birth">
                                        <div className="ProfileBio_Birth_Left">Birth:</div>
                                        <div>
                                            <input
                                                type="date"
                                                value={dateOfBirth}
                                                onChange={(e) => setDateOfBirth(e.target.value)}
                                                className="input-date"
                                            />
                                        </div>
                                    </div>
                                    
                                        
                                    <div className="ProfileBio_Description">
                                        <div className="ProfileBio_Description_Left">Description:</div>
                                        <div>
                                            <textarea
                                                value={preferences}
                                                onChange={(e) => setPreferences(e.target.value)}
                                                className="input-description"
                                            />
                                        </div>
                                    </div>

                                   
                                </div>
                                <div onClick={handleSave} className="Brown_Profile_Button">Save Changes</div>
                            </div>
                            

                        </div>


            )}
            <div>
                {isAuthorized === false ? (
                    <div>UnAuthorized</div>
                ) : (
                    <div>
                        <OrderHistory />
                    </div>
                )}
            </div>
            
        </div>
    );
};



export default Profile;




