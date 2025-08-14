import React from "react";
import { Modal, Button } from "react-bootstrap";

const SupplierModal = ({ show, handleClose, supplier }) => {
    if (!supplier) return null;

    return (
        <Modal show={show} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Tedarikçi Detayları</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p><strong>Firma Adı:</strong> {supplier.name}</p>
                <p><strong>İlgili Kişi:</strong> {supplier.contactName}</p>
                <p><strong>Telefon:</strong> {supplier.phone}</p>
                <p><strong>Email:</strong> {supplier.email}</p>
                <p><strong>Adres:</strong> {supplier.address?.cityName}, {supplier.address?.districtName}</p>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Kapat
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default SupplierModal;
