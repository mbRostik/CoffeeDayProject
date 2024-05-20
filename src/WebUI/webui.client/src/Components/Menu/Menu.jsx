import React from 'react';
import { useState, useEffect, useRef } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import userManager from '../../AuthFiles/authConfig';
import { NavLink } from 'react-router-dom';
import { ThreeDots } from 'react-loader-spinner';
import { useAuth } from './../AuthProvider';
import config from '../../config.json';
import { ToastContainer, toast } from 'react-toastify';
import './Menu.css'
import Bag from '../Bag/Bag.jsx'
import { useBag } from '../Bag/BagContext.jsx';

const Menu = () => {
    const { user, userData, loading, isAuthorized, setLoadingState,
        setIsAuthorizedState,
        setUserState,
        setUserDataState } = useAuth();
    const { setBagProducts } = useBag();
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [categories, setCategories] = useState(null);
    const [products, setProducts] = useState(null);


    async function fetchAllProducts() {
        try {
            const response_posts = await fetch(`${config.apiBaseUrl}/GetAllProducts`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            let response = await response_posts.json();
            setProducts(response);

           
        } catch (error) {
            console.log('There is no friend');
        }
    }

    async function fetchProductsByCategory() {
        console.log(selectedCategory);
        try {
            let Id = selectedCategory;
            const response_posts = await fetch(`${config.apiBaseUrl}/GetProductsByCategory`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Id })

            });
            let response = await response_posts.json();
            setProducts(response);
        } catch (error) {
            console.log('There is no friend');
        }
    }
    const arrayBufferToBase64 = (buffer) => {
        let binary = '';
        const bytes = new Uint8Array(buffer);
        const len = bytes.byteLength;
        for (let i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    };
    async function fetchCategories() {
        try {
            const response = await fetch(`${config.apiBaseUrl}/GetAllCategories`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            const data = await response.json();
            console.log(data);
            setCategories(data);
        } catch (error) {
            console.log('There is no Categories');
        }
    }

    useEffect(() => {
        const asyncFetchData = async () => {
            try {
                setLoadingState(true);
                await fetchCategories();
            } catch (error) {
                console.error("Error while getting friends", error);
            } finally {
                setLoadingState(false);
            }
        };

        asyncFetchData();
    }, []);

    useEffect(() => {
        if (selectedCategory === null) {
            console.log();
        }
        else {
            const asyncFetchData = async () => {
                try {
                    setLoadingState(true);
                    if (selectedCategory === "all") {
                        await fetchAllProducts();
                    }
                    else {
                        await fetchProductsByCategory();
                    }
                } catch (error) {
                    console.error("Error while getting friends", error);
                } finally {
                    setLoadingState(false);
                }
            };
            asyncFetchData();

        }
        

    }, [selectedCategory]);

    const chooseCategory = (id) => {
        console.log(id);
        setSelectedCategory(id);
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

    return (
        <div className="MenuMain">
            <div className="MenuLeftSide">

                <div className="MenuLeftSide_Up">
                    <div className="MenuLeftSide_Up_Text">Categories</div>
                </div>

                <div className="MenuLeftSide_Middle_Categories">
                    {categories === null ?
                        <div>

                        </div>

                        :

                        <div className="MenuLeftSide_Middle_Categories_Inside">
                            <div
                                className={`category-item-middle ${selectedCategory === null ? 'selected' : ''}`}

                            >
                                <p onClick={() => chooseCategory(null)} className="category-item-middle-text">Categories</p>
                            </div>

                            <div
                                className={`category-item-middle ${selectedCategory === "all" ? 'selected' : ''}`}

                            >
                                <p onClick={() => chooseCategory("all")} className="category-item-middle-text">All</p>
                            </div>


                            {categories.map(category => (
                                <div
                                    key={category.id}
                                    className={`category-item-middle ${selectedCategory === category.id ? 'selected' : ''}`}
                                    
                                >
                                    <p onClick={() => chooseCategory(category.id)} className="category-item-middle-text" >{category.name} </p>
                                </div>
                            ))}


                        </div>
                    }
                </div>

                <div className="MenuLeftSide_Down">
                    {selectedCategory === null ?
                        <div className="MenuLeftSide_Down_Categories">
                            {categories === null ?
                                <div>
                                    
                                </div>

                                :

                                <div className="MenuLeftSide_Down_Categories_Inside">
                                    {categories.map(category => (
                                        <div key={category.id} className="category-item" onClick={() => chooseCategory(category.id)}>
                                            <img
                                                src={category.photo ? `data:image/jpeg;base64,${category.photo}` : "/water-bottle.png"}
                                                alt="Picture"
                                                className="category-picture"
                                            />
                                            <p>{category.name}</p>
                                        </div>
                                    ))}
                                </div>
                            }
                        </div>
                        : 

                        <div className="MenuLeftSide_Down_Products">
                            <div>
                               

                                {products === null ?
                                    <div>

                                    </div>

                                    :

                                    <div className="MenuLeftSide_Down_Products">
                                        {products.map(product => (
                                            <div key={product.id} className="product-item">
                                                <div className="product-item-first">
                                                    <img src={product.photo ? `data:image/jpeg;base64` : "/gfd.png"} alt="Picture" className="product-item-picture" />


                                                    <div className="product-item-middletext">
                                                        <div className="product-item-middletext-title">
                                                            {product.name}
                                                        </div>
                                                        <div className="product-item-middletext-text">
                                                            {product.description}

                                                        </div>
                                                        <div className="product-item-price">
                                                            ${product.price}

                                                        </div>
                                                    </div>
                                                </div>
                                               
                                                {isAuthorized === true ?

                                                    <div className="product-item-button" onClick={() => AddTheProduct(product.id)}>
                                                        Order {'>'}
                                                    </div>
                                                    :
                                                    <div></div>
                                                    }
                                                

                                            </div>
                                        ))}
                                    </div>
                                }


                            </div>
                        </div>
                    }
                </div>
                

            </div>

            <div className="MenuRightSide">
                <Bag />
            </div>
        </div>
    );
};



export default Menu;




                                 