import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import { AuthProvider } from './Components/AuthProvider';
import { useNavigate } from 'react-router-dom';
import SignIn_CallbackPage from './AuthFiles/SignIn_CallbackPage';
import SignOut_CallBackPage from './AuthFiles/SignOut_CallBackPage';
import { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, useLocation } from 'react-router-dom';
import NavBar from './Components/NavBar/NavBar';
import Profile from './Components/Profile/Profile';
import ContactUs from './Components/ContactUs/ContactUs';
import Menu from './Components/Menu/Menu';
import Bag from './Components/Bag/Bag';
import { BagProvider } from './Components/Bag/BagContext';
import MainPageBookTable from './Components/MainPage/MainPageBookTable/MainPageBookTable.jsx';

const root = ReactDOM.createRoot(document.getElementById('root'));

function AppContainer() {
    return (
        <div className="All_container">
            <div className="Main_container">
                <div className="Up_Bar">
                    <NavBar />
                </div>
                <div className="Centre_Div">
                    <div className="Centre">
                        <Routes>
                            
                            <Route path="/order-table" element={<MainPageBookTable />} />
                            <Route path="/signin-oidc" element={<SignIn_CallbackPage />} />
                            <Route path="/signout-callback-oidc" element={<SignOut_CallBackPage />} />
                            <Route path="/profile" element={<Profile />} />
                            <Route path="/contact-us" element={<ContactUs />} />
                            <Route path="/menu" element={<Menu />} />
                            <Route path="/bag" element={<Bag />} />
                            <Route path="/" element={<App />} />
                        </Routes>
                    </div>
                </div>
            </div>
        </div>
    );
}

root.render(
    <React.StrictMode>
        <AuthProvider>
            <BagProvider>
            <Router>
                <AppContainer />
                </Router>
            </BagProvider>
        </AuthProvider>
    </React.StrictMode>
);