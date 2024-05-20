import React from 'react';
import './MenuElement.css';

const MenuElement = ({ props } ) => {
    return (
        <div className="coffee-component">
            <img src="gfd.png" alt="Coffee Americano" className="coffee-image" />
            <div className="coffee-details">
                <div className="main-text">{props.name}  ${props.price}</div>
                <div className="element-description-text">{props.description}</div>
            </div>
        </div>
    );
}

export default MenuElement;
