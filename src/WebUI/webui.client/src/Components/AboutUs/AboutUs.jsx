import React, { useState, useEffect } from 'react';
import { Link, useNavigate, NavLink } from 'react-router-dom';
import userManager from '../../AuthFiles/authConfig';
import { ThreeDots } from 'react-loader-spinner';
import { useAuth } from '../AuthProvider';
import config from '../../config.json';
import { ToastContainer, toast } from 'react-toastify';
import './AboutUs.css';

const AboutUs = () => {
    
    return (
        <div className="aboutus-container">
            <div className="aboutus-content">
                <h1 className="aboutus-title">Welcome to Coffee Day</h1>
                <p className="aboutus-subtitle">Your favorite online coffee store</p>

                <p className="aboutus-text">
                    At <strong>Coffee Day</strong>, we believe coffee is more than just a drink —
                    it's a daily ritual, a warm moment, a shared smile. That’s why we source only the finest beans,
                    prepare with care, and deliver right to your door.
                </p>

                <p className="aboutus-text">
                    Whether you're into strong espresso, smooth lattes, or sweet milkshakes, our curated menu has
                    something for every taste. Our products are made with love and attention to detail, and we’re
                    proud to bring café-quality coffee to your home.
                </p>

                <p className="aboutus-text">
                    Have questions, ideas, or just want to say hi? Reach out any time — we love hearing from fellow coffee lovers!
                </p>

                <div className="aboutus-contact">
                    <p>📧 Email: hello@coffeeday.com</p>
                    <p>📍 Location: Online-only – everywhere coffee lovers are</p>
                </div>
            </div>
        </div>
    );
};

export default AboutUs;