import { useEffect, useState } from 'react';
import './BookTable.css';
import userManager from '../../AuthFiles/authConfig';
function BookTable() {
    const [selectedDate, setSelectedDate] = useState('');
    const [peopleCount, setPeopleCount] = useState(1);
    const [availability, setAvailability] = useState([]);
    const [hours, setHours] = useState([]);
    const [loading, setLoading] = useState(false);
    const [refreshTrigger, setRefreshTrigger] = useState(0);

    const [modalOpen, setModalOpen] = useState(false);
    const [modalData, setModalData] = useState(null);
    const [formData, setFormData] = useState({
        name: '',
        email: '',
        phoneNumber: '',
        note: ''
    });
    const [successModalOpen, setSuccessModalOpen] = useState(false);
    const [errorModalOpen, setErrorModalOpen] = useState(false);

    const [cancelModalOpen, setCancelModalOpen] = useState(false);
    const [cancelData, setCancelData] = useState(null);

    const isInactiveHour = (hour) => {
        const hourNumber = parseInt(hour.split(':')[0]);
        return hourNumber >= 23 || hourNumber < 6;
    };

    useEffect(() => {
        const today = new Date();
        today.setDate(today.getDate() + 1);
        const formatted = today.toISOString().split('T')[0];
        setSelectedDate(formatted);
    }, []);


    useEffect(() => {
        if (!selectedDate || peopleCount < 1 || peopleCount > 8) return;

        const fetchAvailability = async () => {
            setLoading(true);
            try {
                const user = await userManager.getUser();
                const accessToken = user?.access_token;

                const res = await fetch(
                    `https://localhost:7071/table/availability?date=${selectedDate}&peopleCount=${peopleCount}`,
                    {
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': `Bearer ${accessToken}`
                        }
                    }
                );

                const data = await res.json();
                setAvailability(data.tables);
                setHours(data.hours);
            } catch (err) {
                console.error('Failed to fetch availability:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchAvailability();
    }, [selectedDate, peopleCount, refreshTrigger]);

    const cancelReservation = async () => {
        try {
            const user = await userManager.getUser();
            const accessToken = user?.access_token;

            const res = await fetch('https://localhost:7071/table/cancel', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                },
                body: JSON.stringify({
                    date: selectedDate,
                    hour: cancelData.hour,
                    tableNumber: cancelData.tableNumber
                })
            });

            if (res.ok) {
                setCancelModalOpen(false);
                setRefreshTrigger(prev => prev + 1);
            } else {
                console.error('Failed to cancel reservation');
            }
        } catch (err) {
            console.error('Cancel error:', err);
        }
    };

    const handleCellClick = (tableNumber, hour) => {
        setModalData({ tableNumber, hour });
        setModalOpen(true);
    };

    const submitReservation = async () => {
        const payload = {
            date: selectedDate,
            startHour: modalData.hour,
            peopleCount,
            tableNumber: modalData.tableNumber.toString(),
            name: formData.name,
            email: formData.email,
            phoneNumber: formData.phoneNumber,
            note: formData.note
        };

        try {
            const user = await userManager.getUser();
            const accessToken = user?.access_token;

            const res = await fetch('https://localhost:7071/table/reserve', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                },
                body: JSON.stringify(payload)
            });

            if (res.ok) {
                setModalOpen(false);
                setSuccessModalOpen(true);
            } else {
                const err = await res.json();
                console.error('Reservation failed:', err);
                setModalOpen(false);
                setErrorModalOpen(true);
            }
        } catch (error) {
            console.error('Unexpected error during reservation:', error);
            setModalOpen(false);
            setErrorModalOpen(true);
        }
    };

    const sortedHours = [...hours].sort((a, b) => {
        const getSortKey = (hourStr) => {
            const h = parseInt(hourStr.split(':')[0]);
            return h < 6 ? h + 24 : h; // put 00:00–05:00 after 23:00
        };
        return getSortKey(a) - getSortKey(b);
    });
    return (
        <div className="MainPageBookTable">
            <div className="MainPageBookTable_Title">Book Your Table</div>
            <div className="MainPageBookTable_Text">
                Reserve your table now and prepare to indulge in culinary delights.
            </div>

            <div className="BookTable_Controls">
                <label>Select date:&nbsp;</label>
                <input
                    type="date"
                    value={selectedDate}
                    onChange={e => setSelectedDate(e.target.value)}
                />
                <label style={{ marginLeft: '20px' }}>People:&nbsp;</label>
                <input
                    type="number"
                    min={1}
                    max={8}
                    value={peopleCount}
                    onChange={e => setPeopleCount(parseInt(e.target.value))}
                    style={{
                        width: '60px',
                        padding: '6px',
                        borderRadius: '6px',
                        border: '2px solid #734022',
                        backgroundColor: '#FFF3E8',
                        color: '#221004'
                    }}
                />
            </div>

            {loading && <div className="BookTable_Loading">Loading availability...</div>}

            {selectedDate && !loading && (
                <div className="BookTable_Grid">
                    <table>
                        <thead>
                            <tr>
                                <th>Table #</th>
                                {[...hours].sort((a, b) => {
                                    const getSortKey = (hourStr) => {
                                        const h = parseInt(hourStr.split(':')[0]);
                                        return h < 6 ? h + 24 : h; // move 0-5 to the end
                                    };
                                    return getSortKey(a) - getSortKey(b);
                                }).map(hour => (
                                    <th key={hour}>{hour}</th>
                                ))}
                            </tr>
                        </thead>
                        <tbody>
                            {availability.map((table, index) => (
                                <tr key={table.tableNumber}>
                                    <td>{index + 1}</td>
                                    {sortedHours.map(hour => (
                                        <td
                                            key={hour}
                                            className={
                                                isInactiveHour(hour)
                                                    ? 'inactive-hour'
                                                    : table.hourReservedByUser[hour]
                                                        ? 'reserved-by-user'
                                                        : table.hourAvailability[hour]
                                                            ? 'available'
                                                            : 'reserved'
                                            }
                                            title={
                                                isInactiveHour(hour)
                                                    ? 'Unavailable (inactive hours)'
                                                    : table.hourReservedByUser[hour]
                                                        ? 'Your Reservation'
                                                        : table.hourAvailability[hour]
                                                            ? 'Available'
                                                            : 'Reserved'
                                            }
                                            onClick={() => {
                                                if (isInactiveHour(hour)) return;
                                                if (table.hourReservedByUser[hour]) {
                                                    setCancelData({ tableNumber: table.tableNumber, hour });
                                                    setCancelModalOpen(true);
                                                } else if (table.hourAvailability[hour]) {
                                                    handleCellClick(table.tableNumber, hour);
                                                }
                                            }}
                                        >
                                            {isInactiveHour(hour)
                                                ? '🚫'
                                                : table.hourReservedByUser[hour]
                                                    ? '★'
                                                    : table.hourAvailability[hour]
                                                        ? '✓'
                                                        : '✘'}
                                        </td>
                                    ))}
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}

            {modalOpen && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h3>Reserve Table #{modalData.tableNumber} at {modalData.hour}</h3>
                        <input
                            placeholder="Your Name"
                            value={formData.name}
                            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                        />
                        <input
                            placeholder="Email"
                            value={formData.email}
                            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                        />
                        <input
                            placeholder="Phone Number"
                            value={formData.phoneNumber}
                            onChange={(e) => setFormData({ ...formData, phoneNumber: e.target.value })}
                        />
                        <textarea
                            placeholder="Note (optional)"
                            value={formData.note}
                            onChange={(e) => setFormData({ ...formData, note: e.target.value })}
                        ></textarea>
                        <div className="modal-buttons">
                            <button onClick={submitReservation}>Reserve</button>
                            <button onClick={() => setModalOpen(false)}>Cancel</button>
                        </div>
                    </div>
                </div>
            )}
            {successModalOpen && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h3 style={{ color: '#221004' }}>✅ Reservation Successful!</h3>
                        <p>Your table has been reserved successfully.</p>
                        <button
                            onClick={() => {
                                setSuccessModalOpen(false);
                                setRefreshTrigger(prev => prev + 1);
                            }}
                        >
                            OK
                        </button>
                    </div>
                </div>
            )}

            {errorModalOpen && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h3 style={{ color: 'red' }}>❌ Reservation Failed</h3>
                        <p>Please check the form data and try again.</p>
                        <button onClick={() => setErrorModalOpen(false)}>Close</button>
                    </div>
                </div>
            )}
            {cancelModalOpen && (
                <div className="modal-overlay">
                    <div className="modal" style={{
                        padding: '24px',
                        backgroundColor: '#fff8f0',
                        borderRadius: '12px',
                        boxShadow: '0 4px 12px rgba(0,0,0,0.15)',
                        color: '#221004',
                        textAlign: 'center',
                        maxWidth: '400px',
                        margin: 'auto'
                    }}>
                        <h3 style={{ marginBottom: '12px', fontSize: '20px', color: '#734022' }}>🗑 Cancel Reservation</h3>
                        <p style={{ marginBottom: '24px', fontSize: '16px' }}>
                            Are you sure you want to cancel your reservation at <strong>Table #{cancelData.tableNumber}</strong> at <strong>{cancelData.hour}</strong>?
                        </p>
                        <div className="modal-buttons" style={{ display: 'flex', justifyContent: 'center', gap: '16px' }}>
                            <button
                                onClick={cancelReservation}
                                style={{
                                    padding: '8px 16px',
                                    backgroundColor: '#c0392b',
                                    color: 'white',
                                    border: 'none',
                                    borderRadius: '8px',
                                    cursor: 'pointer'
                                }}
                            >
                                Yes, Cancel
                            </button>
                            <button
                                onClick={() => setCancelModalOpen(false)}
                                style={{
                                    padding: '8px 16px',
                                    backgroundColor: '#bdc3c7',
                                    color: '#221004',
                                    border: 'none',
                                    borderRadius: '8px',
                                    cursor: 'pointer'
                                }}
                            >
                                No
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default BookTable;
