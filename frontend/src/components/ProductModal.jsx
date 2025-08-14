import React from "react";
import { Modal, Button } from "react-bootstrap";
import { useNavigate } from 'react-router-dom';

const ProductModal = ({ show, handleClose, product }) => {
    if (!product) return null;

    const role = localStorage.getItem("role");
    const navigate = useNavigate();

    return (
        <Modal show={show} onHide={handleClose} size="lg">
            <Modal.Header closeButton>
                <Modal.Title>Detaylar</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p><strong>İsim:</strong> {product.name}</p>
                <p><strong>Açıklama:</strong> {product.description}</p>
                <p><strong>Fiyat:</strong> {product.unitPrice} ₺</p>
                <p><strong>Stok:</strong> {product.unitsInStock}</p>

                <hr />
                <h5>Kategori</h5>
                <p><strong>Adı:</strong> {product.category.name}</p>
                <p><strong>Açıklama:</strong> {product.category.description}</p>

                <hr />
                <h5>Tedarikçi</h5>
                <p><strong>Firma:</strong> {product.supplier.name}</p>
                <p><strong>İlgili Kişi:</strong> {product.supplier.contactName}</p>
                <p><strong>Telefon:</strong> {product.supplier.phone}</p>
                <p><strong>Email:</strong> {product.supplier.email}</p>
                <p><strong>Adres:</strong> {product.supplier.address.cityName}, {product.supplier.address.districtName}</p>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="info"
                    onClick={() => navigate(`/stock-transaction?productId=${product.id}`)}>
                    Stok İşlemleri
                </Button>
                <Button variant="secondary" onClick={handleClose}>Kapat</Button>
            </Modal.Footer>
        </Modal>
    );
};

export default ProductModal;
