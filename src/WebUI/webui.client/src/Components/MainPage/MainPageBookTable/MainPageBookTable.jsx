import { useEffect, useState } from 'react';
import './MainPageBookTable.css';

function MainPageBookTable() {
    const [name, setName] = useState("");
    const [phone, setPhone] = useState("");
    const [email, setEmail] = useState("");
    const [date, setDate] = useState("");
    const [time, setTime] = useState("16:00");
    const [persons, setPersons] = useState("");
    const [message, setMessage] = useState("");
    const [spamResult, setSpamResult] = useState(null);

    const handlePredictSpam = async () => {
        if (!message) {
            setSpamResult("");
            return;
        }

        try {
            console.log(message);
            const response = await fetch('http://127.0.0.1:5000/predict_spam', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ text: message }),
            });
            const data = await response.json();

            if (data.error) {
                setSpamResult(data.error);
            } else {
                if (data.prediction === 'spam') {
                    setSpamResult("Warning: Your message may be classified as spam.");
                } else {
                    setSpamResult("");
                    setName("");
                    setPhone("");
                    setEmail("");
                    setDate("");
                    setTime("16:00");
                    setPersons("");
                    setMessage("");
                }
            }
        } catch (error) {
            setSpamResult("An error occurred while checking for spam.");
        }
    };




    return (
        <div className="MainPageBookTable">
            <div className="MainPageBookTable_Title">
                Book Your Table
            </div>
            <div className="MainPageBookTable_Text">
                Reserve your table now and prepare to indulge in culinary delights.
            </div>
            <div className="MainPageBookTable_Form">
                <form className="booking-form" onSubmit={(e) => e.preventDefault()}>
                    <div className="uppart-booking-form">
                        <div className="booking-form-left">
                            <div className="input-group">
                                <input
                                    type="text"
                                    className="input-field"
                                    placeholder="Your Name"
                                    value={name}
                                    onChange={(e) => setName(e.target.value)}
                                />
                            </div>
                            <div className="input-group">
                                <input
                                    type="text"
                                    className="input-field"
                                    placeholder="Your Phone"
                                    value={phone}
                                    onChange={(e) => setPhone(e.target.value)}
                                />
                            </div>
                            <div className="input-group">
                                <input
                                    type="email"
                                    className="input-field"
                                    placeholder="Email Address"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                />
                            </div>
                        </div>

                        <div className="booking-form-right">
                            <div className="input-group">
                                <input
                                    type="date"
                                    className="input-field"
                                    value={date}
                                    onChange={(e) => setDate(e.target.value)}
                                />
                            </div>
                            <div className="input-group">
                                <input
                                    type="time"
                                    className="input-field"
                                    value={time}
                                    onChange={(e) => setTime(e.target.value)}
                                />
                            </div>
                            <div className="input-group">
                                <input
                                    type="number"
                                    className="input-field"
                                    placeholder="02 Person"
                                    value={persons}
                                    onChange={(e) => setPersons(e.target.value)}
                                />
                            </div>
                        </div>
                    </div>
                    <div className="input-group">
                        <input
                            type="text"
                            className="input-field-message"
                            placeholder="Your Message..."
                            value={message}
                            onChange={(e) => setMessage(e.target.value)}
                        />
                    </div>
                    <div className="Brown_Big_Button" onClick={handlePredictSpam}>
                        Book Now
                    </div>
                    <br></br>
                    <div className="MainPageBookTable_Text" style={{ color: 'red' }}>{spamResult}</div>
                </form>
            </div>
        </div>
    );
}

export default MainPageBookTable;