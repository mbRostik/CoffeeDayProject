import React from 'react';
import { useState, useEffect, useRef } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import userManager from '../../AuthFiles/authConfig';
import { NavLink } from 'react-router-dom';
import { ThreeDots } from 'react-loader-spinner';
import { useAuth } from './../AuthProvider';
import config from '../../config.json';
import { ToastContainer, toast } from 'react-toastify';
import './ContactUs.css'

const ContactUs = () => {
    const [isHovered, setIsHovered] = useState(false);
    const { user, userData, loading, isAuthorized, setLoadingState,
        setIsAuthorizedState,
        setUserState,
        setUserDataState } = useAuth();
    const [isSpamMessage, setIsSpamMessage] = useState(false); 


    useEffect(() => {
        try {
            setName(userData.name);
            setEmail(userData.email);
        }
        catch {
            console.log("Excheption while setting user info");
        }

    }, [isAuthorized]);

    const [user_Message, setMessage] = useState(null);
    const [email, setEmail] = useState(null);
    const [name, setName] = useState(null);


    const handleSubmit = async () => {
        setIsSpamMessage(false);
        setLoadingState(true);
        const updatedUserData = {
            name,
            email,
            user_Message
        };

        try {
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/contactUs`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedUserData)
            });

            if (response.ok) {

                setName(null);
                setEmail(null);
                setMessage(null);
            } else if (response.status === 400) {
                setIsSpamMessage(true);
            }
            else {
                throw new Error('Failed to send message');
            }
        } catch (error) {
            console.error('Error while sending message:', error);
            alert('An error occurred while updating the prfefefefefefefefefeofile.');
        } finally {
            setLoadingState(false);
        }
    };


    return (
        <div className="ContactUsMain">
            {loading ? (
                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <ThreeDots color="orange" height={80} width={80} />
                </div>
            ) : (

                <div className="ContactUsDiv">
                        <div className="ContactUs_Up">

                        </div>
                        <div className="ContactUs_Down">
                            <div className="ContactUs_Title">
                                Get in Touch: Lets Brew Up a Conversation
                            </div>
                            <div className="ContactUs_Text">
                                Whether you are a coffee enthusiast seeking brewing tips or a prospective partner interested in collaboration,
                                we are here to hear from you! Drop us a line, and lets explore how we can caffeinate your world together.
                            </div>

                            <form className="custom-contact-form" onSubmit={handleSubmit}>
                                <div className="form-row">
                                    <input
                                        type="text"
                                        className="input-name"
                                        placeholder="Your Name"
                                        value={name}
                                        onChange={e => setName(e.target.value)} 
                                        required
                                    />
                                    <input
                                        type="email"
                                        className="input-email"
                                        placeholder="Email Address"
                                        value={email} 
                                        onChange={e => setEmail(e.target.value)} 
                                        required
                                    />
                                </div>
                                <textarea
                                    className="textarea-message"
                                    placeholder="Your Message..."
                                    value={user_Message} 
                                    onChange={e => setMessage(e.target.value)} 
                                    required
                                ></textarea>
                                <button type="submit" className="submit-button">Submit</button>
                                {isSpamMessage && (
                                    <div className="spam-warning-text" style={{ color: 'red', marginTop: '10px' }}>
                                        Your message was recognized as spam.
                                    </div>
                                )}
                            </form>
                        </div>
                </div>
            )}
        </div>
    );
};



export default ContactUs;




