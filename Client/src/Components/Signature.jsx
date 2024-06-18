import React from 'react';

export default function Signature() {
    const currentDate = new Date();
    const actualYear = currentDate.getFullYear();

    return (
        <div className='signature'>
            Copyright © {actualYear} DelTSZ | Designed by kC0d3
        </div>
    );
}