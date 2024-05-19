import React from 'react';
import { useState, useEffect, useRef } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import userManager from '../../AuthFiles/authConfig';
import { NavLink } from 'react-router-dom';
import { ThreeDots } from 'react-loader-spinner';
import { useAuth } from '../AuthProvider';
import config from '../../config.json'; 
import '../Styles/NavBarStyles.css'

const NavBar = () => {
    const { user, userData, loading, isAuthorized } = useAuth();
   
    const onLogin = () => {
        userManager.signinRedirect();
    };
    return (
        <div className="NavBarMain">
            {loading ? (
                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <ThreeDots color="orange" height={80} width={80} />
                </div>
            ) : isAuthorized === false ? (
                <div className="NavBarMenuUnAuth">
                        <NavLink to="/" className="nav-item">
                        <div>Home</div>
                    </NavLink>

                        <NavLink to="/about-us" className="nav-item">
                        <div>About Us</div>
                    </NavLink>

                        <NavLink to="/menu" className="nav-item">
                        <div>Menu</div>
                    </NavLink>

                        <NavLink to="/order-table" className="nav-item">
                            <div>Order Table</div>
                        </NavLink>

                        <NavLink to="/" className="nav-item">
                            <div>
                                <img className="nav-logo" src="pngwing.com.png" alt="Logo" />
                            </div>
                        </NavLink>
                        

                        <NavLink to="/shop" className="nav-item">
                        <div>Shop</div>
                    </NavLink>

                        <NavLink to="/contact-us" className="nav-item">
                        <div>Contact Us</div>
                    </NavLink>

                        <NavLink to="/bag" className="nav-item">
                            <div>
                                <img className="nav-bag" src="Vector (3).png" alt="Bag" />
                            </div>
                        </NavLink>

                    <div className="nav-item">
                        <button onClick={onLogin} className="NavBarButton_Login">Login / Sign Up</button>
                    </div>

                </div>
                ) : (

                        <div className="NavBarMenuUnAuth">
                            <NavLink to="/" className="nav-item">
                                <div>Home</div>
                            </NavLink>

                            <NavLink to="/about-us" className="nav-item">
                                <div>About Us</div>
                            </NavLink>

                            <NavLink to="/menu" className="nav-item">
                                <div>Menu</div>
                            </NavLink>

                            <NavLink to="/order-table" className="nav-item">
                                <div>Order Table</div>
                            </NavLink>

                            <NavLink to="/" className="nav-item">
                                <div>
                                    <img className="nav-logo" src="pngwing.com.png" alt="Logo" />
                                </div>
                            </NavLink>


                            <NavLink to="/shop" className="nav-item">
                                <div>Shop</div>
                            </NavLink>

                            <NavLink to="/contact-us" className="nav-item">
                                <div>Contact Us</div>
                            </NavLink>

                            <NavLink to="/bag" className="nav-item">
                                <div>
                                    <img className="nav-bag" src="Vector (3).png" alt="Bag" />
                                </div>
                            </NavLink>
                            
                            <NavLink to="/profile" className="nav-item">
                                <img
                                    className="nav-profile"
                                    src={userData.photo ? `data:image/jpeg;base64,${userData.photo}` : "profile.png"}
                                    alt={userData.nickName || "Profile"}
                                />
                            </NavLink>
                        </div>
            )}
        </div>
    );
};



export default NavBar;



                            
                        