import { useEffect, useState } from 'react';
import './MainPageOurProducts.css';
import userManager from '../../../AuthFiles/authConfig';
import config from '../../../config.json';

function MainPageOurProducts_Products({ prop }) {

        const AddTheProduct = async (id) => {
        try {
            const ProductId = id;
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/AddProductToTheBag`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ ProductId })
            });
        } catch (error) {
            console.log('There is no friend');
        }
    };


    return (
        <div className="MainPageOurProducts_Product">
            <div>
                <img src={prop.photo ? `data:image/jpeg;base64` : "/cup.png"} alt="Picture" className="MainPageOurProduct_Product_Cup" />

            </div>
            <div className="MainPageOurProduct_Product_Blend">
                {prop.description}
            </div>

            <div className="MainPageOurProduct_Product_Name">
                {prop.name}
            </div>

            <div className="MainPageOurProduct_Product_Price">
                ${prop.price}
            </div>

            <div className="Brown_Medium_Button" onClick={() => AddTheProduct(prop.id)}>Add to card</div>

        </div>
    );
}

export default MainPageOurProducts_Products;