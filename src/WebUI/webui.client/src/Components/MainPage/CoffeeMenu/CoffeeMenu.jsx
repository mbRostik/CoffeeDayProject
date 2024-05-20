import React from 'react';
import './Menu.css';
import MenuElement from './MenuElement/MenuElement';
import { useEffect, useState } from 'react';
import { useAuth } from './../../AuthProvider';
import config from '../../../config.json';
import { NavLink } from 'react-router-dom';

const CoffeeMenu = () => {

    const [menuItems, setMenuItems] = useState(null);
    const { user, userData, loading, isAuthorized, setLoadingState,
        setIsAuthorizedState,
        setUserState,
        setUserDataState } = useAuth();

    useEffect(() => {
        const asyncFetchData = async () => {
            try {
                setLoadingState(true);
                await fetchAllProducts();
            } catch (error) {
                console.error("Error while getting friends", error);
            } finally {
                setLoadingState(false);
            }
        };

        asyncFetchData();
    }, []);
    async function fetchAllProducts() {
        try {
            const response_posts = await fetch(`${config.apiBaseUrl}/GetAllProducts`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            let response = await response_posts.json();
            setMenuItems(response);


        } catch (error) {
            console.log('There is no friend');
        }
    }
    return (
        <div className="main-coffee-menu">
            <div className="coffee-menu">
                <div className="head-text">Coffee Menu</div>
                <div className="description-text">Discover the essence of coffee in every sip. Explore our menu.</div>
            </div>
            {menuItems !== null && (
                <div className="main-coffee-components">
                    <div className="coffee-components">
                        {menuItems.slice(-5).map((item, index) => (
                            <MenuElement key={index} props={item} />
                        ))}
                    </div>
                    <div className="coffee-components">
                        {menuItems.slice(0, 5).map((item, index) => (
                            <MenuElement key={index} props={item} />
                        ))}
                    </div>
                </div>       
                )}
                
            <div className="main-coffee-menu-buttons">
                <NavLink to="/menu" className="Brown_Small_Button">
                    <div>Explore More</div>
                </NavLink>
            </div>
        </div>
    );
};

export default CoffeeMenu;