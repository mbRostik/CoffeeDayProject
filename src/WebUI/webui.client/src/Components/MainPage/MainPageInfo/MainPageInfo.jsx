import { useEffect, useState } from 'react';
import './MainPageInfo.css';
import { useAuth } from '../../AuthProvider';
import { NavLink } from 'react-router-dom';

function MainPageInfo() {
    const { user, userData, loading, isAuthorized } = useAuth();

    return (
        <div className="MainPageInfo">
            <div className="MainPageInfo_FirstContainer">

                <div className="MainPageInfo_FC_LeftSide">
                    <div className="MainPageInfo_FC_Title">
                        Coffee Day
                    </div>
                    <div className="MainPageInfo_FC_SecondTitle">
                        An Online coffee store
                    </div>
                    <div className="MainPageInfo_FC_Text">
                        <div>Bringing the aroma right to your doorstep.</div>
                        <div>Our beans await roasting until your order is received.</div>
                        <div>Every single order is meticulously roasted and swiftly delivered the same day.</div>
                        </div>

                    <div className="MainPageInfo_FC_Buttons">
                        <NavLink to="/menu" className="Brown_Small_Button">
                            <div>Explore our products {'>'}</div>
                        </NavLink>
                        
                        {!isAuthorized && (
                            <div className="MainPageInfo_LoginButton">Log In / Sign Up</div>
                        )}
                    </div>
                    <div className="MainPageInfo_FC_Numbers"></div>
                </div>
                <div className="MainPageInfo_FC_RightSide_Cup">
                    <div>
                        <img className="MainPageInfo_FC_RightSide_Cup" src="cup.png" alt="Bag" />
                    </div>
                </div>
            </div>

            <div className="Coffee_Line">
                <div className="Coffee_Line_Text">Cooffee Day</div>
                <div className="Coffee_Line_Text">Cooffee Day</div>
                <div className="Coffee_Line_Text">Cooffee Day</div>
                <div className="Coffee_Line_Text">Cooffee Day</div>

            </div>

            <div className="MainPageInfo_SecondContainer">
                <div className="MainPageInfo_SC_Title">
                    An Online coffee store
                </div>
                <div className="MainPageInfo_SC_Text">
                    <div>In the heart of Ukraine, Coffee Day brews more than just coffee; we craft experiences.
                        Since our inception 2024, our journey has been one of passion, precision, and a deep-rooted commitment to quality.
                        What sets us apart isnt just our beans or coffee; its the stories we share, the connections we foster, and the community we embrace.
                        From farm to cup, every step is infused with our dedication to excellence and our love for the craft.
                        Welcome to Coffee Day, where every cup is a chapter in our story, and every sip is an invitation to join us on our flavorful adventure.</div>
                </div>
            </div>

        </div>
    );
}

export default MainPageInfo;