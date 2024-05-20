import React, { useState, useEffect } from 'react';
import { ThreeDots } from 'react-loader-spinner';
import { useAuth } from './../../AuthProvider';
import userManager from '../../../AuthFiles/authConfig';
import config from '../../../config.json';
import './OrderHistory.css';

const OrderHistory = () => {
    const { isAuthorized, loading, setLoadingState } = useAuth();
    const [orderHistory, setOrderHistory] = useState(null);

    const [selectedOrder, setSelectedOrder] = useState(null);
    const [orderDetails, setOrderDetails] = useState(null);

    const fetchOrderDetails = async (Id) => {
        try {
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/GetUserOrderDetails`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Id })
            });
            const responseData = await response.json();
            setOrderDetails(responseData);
        } catch (error) {
            console.log('There is no friend');
        }
    };


    const selectOrderFunc = async (id) => {
        if (id === selectedOrder) {
            setSelectedOrder(null);
            setOrderDetails(null);
        }
        else {
            setSelectedOrder(id);

            await fetchOrderDetails(id);
        }


    };



    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
        return date.toLocaleDateString(undefined, options);
    };
    const fetchOrderHistory = async () => {
        try {
            const accessToken = await userManager.getUser().then(user => user.access_token);
            const response = await fetch(`${config.apiBaseUrl}/GetUserOrders`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                }
            });
            const responseData = await response.json();
            setOrderHistory(responseData);
        } catch (error) {
            console.log('There is no friend');
        }
    };

    useEffect(() => {
        if (isAuthorized) {
            const asyncFetchData = async () => {
                try {
                    setLoadingState(true);
                    await fetchOrderHistory();
                } catch (error) {
                    console.error("Error while getting order history", error);
                } finally {
                    setLoadingState(false);
                }
            };
            asyncFetchData();
        }
    }, []);

    return (
        <div className="OrderMain">
            {loading ? (
                <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <ThreeDots color="orange" height={80} width={80} />
                </div>
            ) : !isAuthorized ? (
                <div>UnAuthorized</div>
            ) : (
                <div className="OrderDiv">
                    {orderHistory === null ? (
                        <div className="OrderItems">
                            There is no order
                        </div>
                    ) : (
                            <div className="OrderItems_Main">
                                <div className="OrderItemsTitle">Order History</div>
                                <div className="OrderItems">
                                    {orderHistory.slice(-5).map(item => (
                                        <div key={item.id} className="OrderItem" onClick={() => selectOrderFunc(item.id)}>

                                            <img src={"/completed-task.png"} alt="Picture" className="OrderImage" />
                                            <div className="Order_Id">Order Number: {item.id}</div>

                                            <div className="Order_DownInformation">
                                                <div className="Order_DownInformation_Date">{formatDate(item.date)}</div>
                                                <div className="Order_DownInformation_Price">${item.summ}</div>

                                            </div>
                                        </div>
                                    ))}
                                        </div>


                                <div className="OrderDetails_Main">

                                            {orderDetails === null ? (<div></div>) : (
                                                <div className="OrderDetails_Main_Items">
                                                    <div className="OrderDetails_Main_Title">
                                                        Order Details:
                                                    </div>
                                                    {orderDetails.map(Product => (
                                                        <div key={Product.id} className="OrderDetails_Main_Item">
                                                            <div className="OrderDetails_Main_Item_LeftSide">
                                                                <img src={Product.productPhoto ? `data:image/jpeg;base64` : "/gfd.png"} alt="Picture" className="OrderDetails_Main_Picture" />

                                                                <div className="OrderDetails_Main_Item_Information">
                                                                    <div className="OrderDetails_Main_Item_Information_Name">{Product.productName}</div>
                                                                    <div className="OrderDetails_Main_Item_Information_Count">Count: {Product.count}</div>

                                                                </div>

                                                                <div className="OrderDetails_Main_Item_Information_Description">{Product.productDescription}</div>
                                                            </div>
                                                            <div className="OrderDetails_Main_Item_RightSide">
                                                                <div className="OrderDetails_Main_Item_Information_TotalPrice">Price: ${Product.count * Product.productPrice}</div>
                                                            </div>

                                                        </div>

                                                    ))}
                                                </div>
                                            
                                            )}
                                           

                                </div>
                            </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default OrderHistory;
