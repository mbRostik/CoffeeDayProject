import { useEffect, useState } from 'react';
import './MainPageBookTable.css';

function MainPageBookTable() {
    return (
        <div className="MainPageBookTable">
            <div className="MainPageBookTable_Title">
                Book Your Table
            </div>
            <div className="MainPageBookTable_Text">
                Reserve your table now and prepare to indulge in culinary delights.
            </div>
            <div className="MainPageBookTable_Form">

                <form className="booking-form">
                    <div className="uppart-booking-form">
                        <div className="booking-form-left">
                            <div className="input-group">
                                <input type="text" className="input-field" placeholder="Your Name" />
                            </div>
                            <div className="input-group">
                                <input type="text" className="input-field" placeholder="Your Phone" />
                            </div>
                            <div className="input-group">
                                <input type="email" className="input-field" placeholder="Email Address" />
                            </div>
                        </div>

                        <div className="booking-form-right">
                            <div className="input-group">
                                <input type="date" className="input-field" />
                            </div>
                            <div className="input-group">
                                <input type="time" className="input-field" value="16:00" />
                            </div>
                            <div className="input-group">
                                <input type="number" className="input-field" placeholder="02 Person" />
                            </div>
                        </div>
                    </div>
                    <div className="input-group">
                        <input type="text" className="input-field-message" placeholder="Your Message..." />

                    </div>
                    <div className="Brown_Big_Button">Book Now</div>
                </form>
            </div>
        </div>
    );
}

export default MainPageBookTable;