import React, { useEffect, useState } from "react";
import { Table, Button, Spinner } from "react-bootstrap";
import { apiFetch } from "../../api";
import ProductModal from "../../components/ProductModal";
import Header from "../../components/Header";
import { useNavigate } from "react-router-dom";

const ProductList = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedProduct, setSelectedProduct] = useState(null);
    const [showModal, setShowModal] = useState(false);

    const navigate = useNavigate();

    const loadProducts = async () => {
        const res = await apiFetch("/api/Product");
        if (res.ok) {
            setProducts(res.data);
        }
        setLoading(false);
    };

    useEffect(() => {
        loadProducts();
    }, []);

    const handleShowDetails = (product) => {
        setSelectedProduct(product);
        setShowModal(true);
    };

    const handleCloseModal = () => {
        setShowModal(false);
        setSelectedProduct(null);
    };

    if (loading) return <Spinner animation="border" className="mt-4" />;

    return (
        <>
            <Header />
            <div className="container mt-4">
                <div className="d-flex justify-content-between align-items-center mb-3">
                    <h2>Ürün Listesi</h2>
                    <button className="btn btn-primary" onClick={() => navigate('/product-create')}>Ürün Ekle</button>
                </div>
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>Adı</th>
                            <th>Açıklama</th>
                            <th>Stok</th>
                            <th>Fiyat</th>
                            <th>İşlem</th>
                        </tr>
                    </thead>
                    <tbody>
                        {products.map((p) => (
                            <tr key={p.id}>
                                <td>{p.name}</td>
                                <td>{p.description}</td>
                                <td>{p.unitsInStock}</td>
                                <td>{p.unitPrice} ₺</td>
                                <td>
                                    <Button onClick={() => handleShowDetails(p)}>
                                        Detay Göster
                                    </Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>

                <ProductModal
                    show={showModal}
                    handleClose={handleCloseModal}
                    product={selectedProduct}
                />
            </div>
        </>
    );
};

export default ProductList;
