import React from 'react';
import './Menu.css';
import MenuElement from './MenuElement/MenuElement';

const CoffeeMenu = () => {
    const menuItems = [
        { name: 'Caffee Americano ....................... $5', description: 'A classic, bold brew that embodies the essence of simplicity.' },
        { name: 'Caffee Americano ....................... $5', description: 'A classic, bold brew that embodies the essence of simplicity.' },
        { name: 'Caffee Americano ....................... $5', description: 'A classic, bold brew that embodies the essence of simplicity.' },
        { name: 'Caffee Americano ....................... $5', description: 'A classic, bold brew that embodies the essence of simplicity.' },        
    ];


    return (
        <div className="main-coffee-menu">
            <div className="coffee-menu">
                <div className="head-text">Coffee Menu</div>
                <div className="description-text">Discover the essence of coffee in every sip. Explore our menu.</div>
            </div>
            <div className="main-coffee-components">
                <div className="coffee-components">
                    {menuItems.map((item, index) => (
                        <MenuElement key={index} props={item} />
                    ))}
                </div>
                <div className="coffee-components">
                    {menuItems.map((item, index) => (
                        <MenuElement key={index} props={item} />
                    ))}
                </div>
            </div>            
            <div className="main-coffee-menu-buttons">
                <div className="Brown_Small_Button">Explore More</div>
            </div>
        </div>
    );
};

export default CoffeeMenu;