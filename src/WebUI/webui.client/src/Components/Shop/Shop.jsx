import React, { useState, useEffect } from 'react';
import './Shop.css';
import userManager from '../../AuthFiles/authConfig';
import ShopBag from './ShopBag';

const Shop = () => {
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [selectedCategoryId, setSelectedCategoryId] = useState(null);
    const [loading, setLoading] = useState(false);
    const [showBag, setShowBag] = useState(false);
    const [buyingProduct, setBuyingProduct] = useState(null);
    const [quantity, setQuantity] = useState(1);
    const [showSuccessPopup, setShowSuccessPopup] = useState(false);

    useEffect(() => {
        const fetchCategories = async () => {
            setLoading(true);
            try {
                const user = await userManager.getUser();
                const accessToken = user?.access_token;

                const res = await fetch('https://localhost:7230/shop/categories', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${accessToken}`
                    }
                });

                const data = await res.json();
                setCategories(data);
            } catch (err) {
                console.error('Failed to fetch categories:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchCategories();
    }, []);

    useEffect(() => {
        if (!selectedCategoryId) return;

        const fetchProducts = async () => {
            setLoading(true);
            try {
                const user = await userManager.getUser();
                const accessToken = user?.access_token;

                const res = await fetch(`https://localhost:7230/shop/products?categoryId=${selectedCategoryId}`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${accessToken}`
                    }
                });

                const data = await res.json();
                setProducts(data);
            } catch (err) {
                console.error('Failed to fetch products:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, [selectedCategoryId]);

    const handleAddToBag = async () => {
        try {
            const user = await userManager.getUser();
            const accessToken = user?.access_token;

            const res = await fetch(`https://localhost:7230/shop/bag/add`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                },
                body: JSON.stringify({
                    productId: buyingProduct.id,
                    quantity
                })
            });

            if (res.ok) {
                setBuyingProduct(null);
                setQuantity(1);
                setShowSuccessPopup(true);
                setTimeout(() => setShowSuccessPopup(false), 2500);
            } else {
                console.error('Failed to add to bag');
            }
        } catch (err) {
            console.error('Error:', err);
        }
    };

    return (
        <div className="shop-container">
            <div className="shop-header">
                <h2 className="shop-title">☕ Welcome to the Coffee Shop</h2>
                <button className="bag-button" onClick={() => setShowBag(true)}>👜 Bag</button>
            </div>

            <div className="category-list">
                {categories.map(category => (
                    <div
                        key={category.id}
                        className={`category-card ${selectedCategoryId === category.id ? 'selected' : ''}`}
                        onClick={() => setSelectedCategoryId(category.id)}
                    >
                        <img
                            src={category.photo?.startsWith('data:image') ? category.photo : `data:image/png;base64,${category.photo}`}
                            alt={category.name}
                            className="category-picture"
                        />
                        <div>{category.name}</div>
                    </div>
                ))}
            </div>

            {selectedCategoryId && (
                <>
                    <h3>Products in Selected Category</h3>
                    <div className="product-list">
                        {products.map(product => (
                            <div key={product.id} className="product-card">
                                <img
                                    src={product.photo?.startsWith('data:image') ? product.photo : `data:image/png;base64,${product.photo}`}
                                    alt={product.name}
                                    className="product-picture"
                                />
                                <h4>{product.name}</h4>
                                <p>{product.description}</p>
                                <p><strong>${product.price.toFixed(2)}</strong></p>
                                <button className="buy-button" onClick={() => setBuyingProduct(product)}>Buy</button>
                            </div>
                        ))}
                    </div>
                </>
            )}

            {buyingProduct && (
                <div className="buy-popup">
                    <div className="buy-popup-content">
                        <img
                            src={buyingProduct.photo?.startsWith('data:image') ? buyingProduct.photo : `data:image/png;base64,${buyingProduct.photo}`}
                            alt={buyingProduct.name}
                            className="buy-popup-image"
                        />
                        <h4>{buyingProduct.name}</h4>
                        <p><strong>${buyingProduct.price.toFixed(2)}</strong></p>
                        <label>
                            Quantity:
                            <input
                                type="number"
                                value={quantity}
                                min={1}
                                onChange={e => setQuantity(parseInt(e.target.value))}
                            />
                        </label>
                        <div className="buy-popup-buttons">
                            <button onClick={handleAddToBag}>Add to Bag</button>
                            <button onClick={() => setBuyingProduct(null)}>Cancel</button>
                        </div>
                    </div>
                </div>
            )}

            {loading && <div className="loading">Loading...</div>}

            {showBag && (
                <div className="bag-modal">
                    <div className="bag-overlay" onClick={() => setShowBag(false)}></div>
                    <div className="bag-content">
                        <ShopBag onClose={() => setShowBag(false)} />
                    </div>
                </div>
            )}
            {showSuccessPopup && (
                <div className="popup-notification">
                    ✅ Product added to your bag!
                </div>
            )}
        </div>
    );
};

export default Shop;
