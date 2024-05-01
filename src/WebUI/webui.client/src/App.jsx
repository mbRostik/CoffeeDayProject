import { useEffect, useState } from 'react';
import './App.css';
import MainPageInfo from './Components/MainPage/MainPageInfo/MainPageInfo.jsx'
import MainPageOurProducts from './Components/MainPage/MainPageOurProducts/MainPageOurProducts.jsx'
import CoffeeMenu from './Components/MainPage/CoffeeMenu/CoffeeMenu.jsx'

function App() {
    return (
        <div className="MainPage">
            <MainPageInfo />
            <div className="Coffee_Line">
                <div className="Coffee_Line_Text">Cooffee Day</div>
                <div className="Coffee_Line_Text">Cooffee Day</div>
                <div className="Coffee_Line_Text">Cooffee Day</div>
                <div className="Coffee_Line_Text">Cooffee Day</div>

            </div>
            <MainPageOurProducts />

            <CoffeeMenu/>
        </div>
    );
}

export default App;