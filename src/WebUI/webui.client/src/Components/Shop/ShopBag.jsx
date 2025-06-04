import React, { useEffect, useState } from 'react';
import './Shop.css';
import userManager from '../../AuthFiles/authConfig';

const ITEMS_PER_PAGE = 2;

const ShopBag = ({ onClose }) => {
    const [bagItems, setBagItems] = useState([]);
    const [loading, setLoading] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [showCheckout, setShowCheckout] = useState(false);
    const [checkoutData, setCheckoutData] = useState({ name: '', phone: '', address: '' });
    const [showSuccessPopup, setShowSuccessPopup] = useState(false);

    const fetchBagItems = async () => {
        setLoading(true);
        try {
            const user = await userManager.getUser();
            const accessToken = user?.access_token;

            const res = await fetch('https://localhost:7230/shop/bag', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${accessToken}`
                }
            });

            if (!res.ok) throw new Error('Failed to fetch bag items');
            const data = await res.json();
            setBagItems(data);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchBagItems();
    }, []);

    const handleRemove = async (productId) => {
        try {
            const user = await userManager.getUser();
            const accessToken = user?.access_token;

            const res = await fetch(`https://localhost:7230/shop/bag/remove`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                },
                body: JSON.stringify({ productId })
            });

            if (res.ok) {
                const updatedItems = bagItems.filter(item => item.productId !== productId);
                setBagItems(updatedItems);
                const maxPage = Math.ceil(updatedItems.length / ITEMS_PER_PAGE);
                if (currentPage > maxPage) setCurrentPage(maxPage);
            } else {
                console.error('Failed to remove item from bag');
            }
        } catch (err) {
            console.error('Error removing item:', err);
        }
    };

    const handleCheckout = async () => {
        try {
            const user = await userManager.getUser();
            const accessToken = user?.access_token;

            const res = await fetch('https://localhost:7230/shop/order', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                },
                body: JSON.stringify(checkoutData)
            });

            if (res.ok) {
                setShowSuccessPopup(true);
                setShowCheckout(false);
                setCheckoutData({ name: '', phone: '', address: '' });
                fetchBagItems();
            } else {
                alert('Failed to place order.');
            }
        } catch (err) {
            console.error('Checkout error:', err);
        }
    };

    const calculateTotal = () =>
        bagItems.reduce((sum, item) => sum + item.quantity * item.price, 0).toFixed(2);

    const totalPages = Math.ceil(bagItems.length / ITEMS_PER_PAGE);
    const paginatedItems = bagItems.slice(
        (currentPage - 1) * ITEMS_PER_PAGE,
        currentPage * ITEMS_PER_PAGE
    );

    return (
        <div className="shop-bag">
            <div className="shop-bag-header">
                <h2>👜 Your Shopping Bag</h2>
                <button className="close-button" onClick={onClose}>✖</button>
            </div>

            {loading && <div className="loading">Fetching your bag...</div>}
            {!loading && bagItems.length === 0 && <p className="empty-bag-msg">Your bag is currently empty.</p>}

            {!loading && bagItems.length > 0 && (
                <>
                    <div className="bag-items">
                        {paginatedItems.map(item => (
                            <div key={item.productId} className="bag-item-card">
                                <img
                                    src={item.photo}
                                    alt={item.name}
                                    className="bag-item-image"
                                />
                                <div className="bag-item-info">
                                    <h4 className="bag-item-name">{item.name}</h4>
                                    <p className="bag-item-detail">Unit Price: <strong>${item.price.toFixed(2)}</strong></p>
                                    <p className="bag-item-detail">Quantity: {item.quantity}</p>
                                    <p className="bag-item-detail">Subtotal: <strong>${(item.price * item.quantity).toFixed(2)}</strong></p>
                                    <button
                                        className="remove-button"
                                        onClick={() => handleRemove(item.productId)}
                                    >
                                        ❌ Remove
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="pagination-controls">
                        <button
                            disabled={currentPage === 1}
                            onClick={() => setCurrentPage(prev => prev - 1)}
                        >⬅ Prev</button>
                        <span>Page {currentPage} / {totalPages}</span>
                        <button
                            disabled={currentPage === totalPages}
                            onClick={() => setCurrentPage(prev => prev + 1)}
                        >Next ➡</button>
                    </div>

                    <div className="bag-total">
                        <span>Total Amount:</span>
                        <strong>${calculateTotal()}</strong>
                    </div>

                    <div className="checkout-section">
                        <button className="checkout-button" onClick={() => setShowCheckout(true)}>💳 Pay</button>
                    </div>

                    {showCheckout && (
                        <div className="checkout-form">
                            <h3>Complete your order</h3>
                            <input
                                type="text"
                                placeholder="Your Name"
                                value={checkoutData.name}
                                onChange={e => setCheckoutData({ ...checkoutData, name: e.target.value })}
                            />
                            <input
                                type="text"
                                placeholder="Phone Number"
                                value={checkoutData.phone}
                                onChange={e => setCheckoutData({ ...checkoutData, phone: e.target.value })}
                            />
                            <input
                                type="text"
                                placeholder="Delivery Address"
                                value={checkoutData.address}
                                onChange={e => setCheckoutData({ ...checkoutData, address: e.target.value })}
                            />
                            <button className="confirm-button" onClick={handleCheckout}>Confirm Order</button>
                            <button className="cancel-button" onClick={() => setShowCheckout(false)}>Cancel</button>
                        </div>
                    )}
                    {showSuccessPopup && (
                        <div className="popup-overlay">
                            <div className="popup-content">
                                <h3>✅ Order Confirmed!</h3>
                                <p>Thank you for your purchase. Your order is being processed.</p>
                                <button onClick={() => setShowSuccessPopup(false)}>Close</button>
                            </div>
                        </div>
                    )}
                </>

            )}
        </div>
    );
};

export default ShopBag;
