import React from 'react';

const Header = () => {
    const handleLogout = () => {
        localStorage.clear();
        window.location.reload();
    };

    return (
        <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div className="container-fluid">
                <a className="navbar-brand" href='/home'>Stok Yönetimi</a>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between w-100">
                    <ul className="navbar-nav flex-grow-1">
                        <li className="nav-item">
                            <a className="nav-link text-dark" href="/product-list">Ürünler</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link text-dark" href="/supplier-list">Tedarikçiler</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link text-dark" href="/logs">Log Yöneticisi</a>
                        </li>
                    </ul>
                    <button className="btn btn-outline-danger" onClick={handleLogout}>
                        Çıkış Yap
                    </button>
                </div>
            </div>
        </nav>
    );
};

export default Header;
