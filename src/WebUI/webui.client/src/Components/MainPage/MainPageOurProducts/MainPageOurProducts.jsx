import { useEffect, useState } from 'react';
import './MainPageOurProducts.css';
import MainPageOurProducts_Products from './MainPageOurProducts_Products.jsx'

function MainPageOurProducts() {
    const menuItems = [
        { category: 'blend', name: 'feqfqefqefq', price: 5 },
        { category: 'blend', name: 'fqefqefq', price: 5 },
        { category: 'blend', name: 'fqefqefq', price: 5 }
    ];

    return (
        <div className="MainPageOurProducts">
            <div className="MainPageOurProducts_Info">
                <div className="MainPageOurProducts_Title">
                    Our Products
                </div>
                <div className="MainPageOurProducts_Text">
                    Discover the Essence of Excellence. Explore our curated selection crafted with passion and precision.
                </div>
            </div>

            <div className="MainPageOurProducts_Products">
                {menuItems.map((item, index) => (
                    <MainPageOurProducts_Products key={index} prop={item} />
                ))}                
            </div>
        </div>
    );
}

export default MainPageOurProducts;