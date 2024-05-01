import { useEffect, useState } from 'react';
import './MainPageOurProducts.css';

function MainPageOurProducts_Products({ prop }) {
    return (
        <div className="MainPageOurProducts_Product">
            <div>
                <img className="MainPageOurProduct_Product_Cup" src="cup.png" alt="Bag" />
            </div>
            <div className="MainPageOurProduct_Product_Blend">
                {prop.category}
            </div>

            <div className="MainPageOurProduct_Product_Name">
                {prop.name}
            </div>

            <div className="MainPageOurProduct_Product_Price">
                ${prop.price}
            </div>

            <div className="Brown_Medium_Button">Add to card</div>

        </div>
    );
}

export default MainPageOurProducts_Products;