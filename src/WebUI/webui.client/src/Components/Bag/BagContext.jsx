import React, { createContext, useContext, useState } from 'react';

const BagContext = createContext();

export const useBag = () => useContext(BagContext);

export const BagProvider = ({ children }) => {
    const [bagProducts, setBagProducts] = useState([]);

    return (
        <BagContext.Provider value={{ bagProducts, setBagProducts }}>
            {children}
        </BagContext.Provider>
    );
};
