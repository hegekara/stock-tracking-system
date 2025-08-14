import React from 'react';
import Header from '../components/Header';

function Home() {
    return (
        <div>
            <Header />
            <div className="container text-center m-5">
                <h1 className="display-4">Stok Takip Sistemi</h1>
                <p className="lead">Stok takibini kolaylaştır.</p>
            </div>
        </div>
    );
}

export default Home;
