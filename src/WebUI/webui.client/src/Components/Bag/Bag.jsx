import React from 'react';
import { useState, useEffect, useRef } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import userManager from '../../AuthFiles/authConfig';
import { NavLink } from 'react-router-dom';
import { ThreeDots } from 'react-loader-spinner';
import { useAuth } from './../AuthProvider';
import config from '../../config.json';
import { ToastContainer, toast } from 'react-toastify';
import './Bag.css'
import { useBag } from './BagContext';

const Bag = () => {
    const { isAuthorized, loading, setLoadingState } = useAuth();
    const { bagProducts, setBagProducts } = useBag();

    async function fetchBagProducts() {
        try {
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/GetUserBag`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                }
            });
            const responseData = await response.json();
            setBagProducts(responseData);
        } catch (error) {
            console.log('There is no friend');
        }
    }

    useEffect(() => {
        if (isAuthorized) {
            const asyncFetchData = async () => {
                try {
                    setLoadingState(true);
                    await fetchBagProducts();
                } catch (error) {
                    console.error("Error while getting friends", error);
                } finally {
                    setLoadingState(false);
                }
            };
            asyncFetchData();
        }
    }, [isAuthorized]);

    const PayOrder = async () => {
        try {
           
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/PayOrder`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                }
            });
            const responseData = await response.json();
            if (responseData === true) {
                await fetchBagProducts();
            }
        } catch (error) {
            console.log('There is no friend');
        }
    };


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
            const responseData = await response.json();
            if (responseData === true) {
                await fetchBagProducts();
            }
        } catch (error) {
            console.log('There is no friend');
        }
    };

    const RemoveTheProduct = async (id) => {
        try {
            const ProductId = id;
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/RemoveProductFromTheBag`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ ProductId })
            });
            const responseData = await response.json();
            if (responseData === true) {
                await fetchBagProducts();
            }
        } catch (error) {
            console.log('There is no friend');
        }
    };

    const RemoveAllProductsFromTheBag = async (id) => {
        try {
            const ProductId = id;
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/RemoveAllProductsFromTheBag`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ ProductId })
            });
            const responseData = await response.json();
            if (responseData === true) {
                await fetchBagProducts();
            }
        } catch (error) {
            console.log('There is no friend');
        }
    };

    const totalValues = bagProducts.reduce((acc, bagProduct) => {
        acc.totalPrice += bagProduct.price * bagProduct.count;
        acc.totalCount += bagProduct.count;
        return acc;
    }, { totalPrice: 0, totalCount: 0 });

    return (
        <div className="BagMain">
            {loading ? (
                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <ThreeDots color="orange" height={80} width={80} />
                </div>
            ) : !isAuthorized ? (
                <div>UnAuthorized</div>
            ) : (
                <div className="BagDiv">
                    {bagProducts.length === 0 ? (
                                <div className="BagItems">
                                    <div className="BagItems_Title">Your bag:</div>
                                    <div className="Bag_Empty_Div">
                                        <img src={"/empty-cart.png"} alt="Picture" className="Bag_Empty" />
                                        <div className="Bag_Empty_Div_Title">Your bag is empty</div>
                                    </div>

                                </div>
                    ) : (
                                    <div className="BagItems">
                                        <div className="BagItems_Title">Your bag:</div>
                                        {bagProducts.map(bagProduct => (
                                            <div key={bagProduct.id} className="Bag-Product_Item">

                                                <div className="Bag_Product_Information">
                                                    <img src={bagProduct.photo ? `data:image/jpeg;base64` : "/gfd.png"} alt="Picture" className="Bag_Product_Information_Photo" />
                                                    <div className="Bag_Product_Information_Total">

                                                        <div className="Bag_Product_Information_Up">
                                                            <div className="Bag_Product_Information_Up_Name">{bagProduct.name}</div>

                                                            <div className="Bag_Product_Information_Up_Description">{bagProduct.description}</div>
                                                            </div>

                                                        <div className="Bag_Product_Information_Down">
                                                            <div className="Bag_Product_Information_Down_Price">Unit price: ${bagProduct.price}</div>
                                                            <div className="Bag_Product_Information_Down_Control">
                                                                <div className="Bag_Product_Information_Down_Minus" onClick={() => RemoveTheProduct(bagProduct.productId)}>--</div>

                                                                <div className="Bag_Product_Information_Down_Count">{bagProduct.count}</div>

                                                                <div className="Bag_Product_Information_Down_Plus" onClick={() => AddTheProduct(bagProduct.productId)}>++</div>
                                                            </div>
                                                            
                                                            </div>
                                                        
                                                    </div>


                                                </div>

                                                <div className="Bag_Product_Right">
                                                    <div className="Bag_Product_Right_TotalPrice">${bagProduct.price * bagProduct.count}</div>
                                                    <img src={"/delete.png"} alt="Picture" className="Bag_Product_Right_Delete" onClick={() => RemoveAllProductsFromTheBag(bagProduct.productId)} />

                                                </div>

                                                
                                            </div>

                                        ))}
                                        <div className="Bag_Down_All">
                                            <div className="Bag_Down_Total">

                                                <div className="Bag_Down_Total_Left">
                                                    <div className="Bag_Down_Total_Up">
                                                        Quantity
                                                    </div>
                                                    <div className="Bag_Down_Total_Down">
                                                        {totalValues.totalCount}
                                                    </div>
                                                </div>

                                                <div className="Bag_Down_Total_Right">
                                                    <div className="Bag_Down_Total_Up">
                                                        Total Price
                                                    </div>
                                                    <div className="Bag_Down_Total_Down">
                                                        ${totalValues.totalPrice.toFixed(2)}
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="bag-nutton-order" onClick={() => PayOrder()}>
                                                Order {'>'}
                                            </div>
                                        </div>
                                    </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default Bag;




