import { useEffect, useState } from 'react';
import './MainPageOurProducts.css';
import MainPageOurProducts_Products from './MainPageOurProducts_Products.jsx'
import { useAuth } from './../../AuthProvider';
import config from '../../../config.json';
function MainPageOurProducts() {
    const [products, setProducts] = useState(null);
    const { user, userData, loading, isAuthorized, setLoadingState,
        setIsAuthorizedState,
        setUserState,
        setUserDataState } = useAuth();

    useEffect(() => {
        const asyncFetchData = async () => {
            try {
                setLoadingState(true);
                await fetchAllProducts();
            } catch (error) {
                console.error("Error while getting friends", error);
            } finally {
                setLoadingState(false);
            }
        };

        asyncFetchData();
    }, []);
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
            {products !== null && (
                <div className="MainPageOurProducts_Products">
                    {products.slice(0, 3).map((item, index) => (
                        <MainPageOurProducts_Products key={index} prop={item} />
                    ))}
                </div>
            )}
        </div>
    );
}

export default MainPageOurProducts;