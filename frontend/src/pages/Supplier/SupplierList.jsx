import React, { useEffect, useState } from "react";
import { Table, Button, Spinner } from "react-bootstrap";
import { apiFetch } from "../../api";
import SupplierModal from "../../components/SupplierModal";
import Header from "../../components/Header";
import { useNavigate } from "react-router-dom";

const SupplierList = () => {
    const [suppliers, setSuppliers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedSupplier, setSelectedSupplier] = useState(null);
    const [showModal, setShowModal] = useState(false);

    var navigate = useNavigate();

    const fetchSuppliers = async () => {
        const res = await apiFetch("/api/Supplier");
        if (res.ok) {
            setSuppliers(res.data);
            console.log(suppliers);
            
        }
        setLoading(false);
    };

    useEffect(() => {
        fetchSuppliers();
    }, []);

    const handleShowDetails = (supplier) => {
        setSelectedSupplier(supplier);
        setShowModal(true);
    };

    const handleCloseModal = () => {
        setSelectedSupplier(null);
        setShowModal(false);
    };

    if (loading) return <Spinner animation="border" className="mt-4" />;

    return (
        <>
            <Header />
            <div className="container mt-4 px-5">
                <div className="d-flex justify-content-between align-items-center mb-3">
                    <h2>Tedarikçi Listesi</h2>
                    <button className="btn btn-primary" onClick={() => navigate('/supplier-create')}>Tedarikçi Oluştur</button>
                </div>
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>Adres</th>
                            <th>Firma Adı</th>
                            <th>İlgili Kişi</th>
                            <th>Telefon</th>
                            <th>Email</th>
                            <th>İşlem</th>
                        </tr>
                    </thead>
                    <tbody>
                        {suppliers.map((s) => (
                            <tr key={s.id}>
                                <td>{s.address.cityName}</td>
                                <td>{s.name}</td>
                                <td>{s.contactName}</td>
                                <td>{s.phone}</td>
                                <td>{s.email}</td>
                                <td>
                                    <Button onClick={() => handleShowDetails(s)}>
                                        Detay Göster
                                    </Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>

                <SupplierModal
                    show={showModal}
                    handleClose={handleCloseModal}
                    supplier={selectedSupplier}
                />
            </div>
        </>
    );
};

export default SupplierList;
