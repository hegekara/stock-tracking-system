import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { Form, Button, Card, Spinner, Alert } from "react-bootstrap";
import { apiFetch } from "../../api";
import Header from "../../components/Header";

const StockTransaction = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const queryParams = new URLSearchParams(location.search);
    const productId = queryParams.get("productId");

    const [product, setProduct] = useState(null);
    const [quantity, setQuantity] = useState("");
    const [loading, setLoading] = useState(true);
    const [message, setMessage] = useState(null);
    const [error, setError] = useState(null);

    const fetchProduct = async () => {
        const res = await apiFetch(`/api/Product/${productId}`);
        if (res.ok) {
            setProduct(res.data);
        } else {
            setError("Ürün bilgisi alınamadı.");
        }
        setLoading(false);
    };

    useEffect(() => {
        fetchProduct();
    }, [productId]);

    const handleStockChange = async (type) => {
        if (!quantity || isNaN(quantity) || quantity <= 0) {
            setError("Lütfen geçerli bir miktar girin.");
            return;
        }
        setError(null);
        setMessage(null);

        const endpoint =
            type === "add" ? "/api/transaction/add-stock" : "/api/transaction/remove-stock";

        const res = await apiFetch(endpoint, {
            method: "POST",
            body: {
                quantity: parseInt(quantity, 10),
                productId: parseInt(productId, 10),
            },
        });

        if (res.ok) {
            setMessage(`Stok başarıyla ${type === "add" ? "artırıldı" : "azaltıldı"}.`);
            const updated = await apiFetch(`/api/Product/${productId}`);
            if (updated.ok) {
                setProduct(updated.data);
            }
            setQuantity("");
        } else {
            setError("Stok işlemi başarısız oldu.");
        }
    };

    if (loading) return <Spinner animation="border" className="mt-4" />;

    return (
        <>
            <Header />
            <div className="container mt-4 px-5">
                <Card>
                    <Card.Header>
                        <h4>Stok İşlemleri - {product?.name}</h4>
                    </Card.Header>
                    <Card.Body>
                        {error && <Alert variant="danger">{error}</Alert>}
                        {message && <Alert variant="success">{message}</Alert>}

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

                        <Form>
                            <Form.Group className="mb-3">
                                <Form.Label>Miktar</Form.Label>
                                <Form.Control
                                    type="number"
                                    value={quantity}
                                    onChange={(e) => setQuantity(e.target.value)}
                                    placeholder="Miktar girin"
                                />
                            </Form.Group>

                            <div className="d-flex gap-2">
                                <Button variant="success" onClick={() => handleStockChange("add")}>
                                    Stok Ekle
                                </Button>
                                <Button variant="danger" onClick={() => handleStockChange("remove")}>
                                    Stok Azalt
                                </Button>
                                <Button variant="secondary" onClick={() => navigate(-1)}>
                                    Geri Dön
                                </Button>
                            </div>
                        </Form>
                    </Card.Body>
                </Card>
            </div>
        </>
    );
};

export default StockTransaction;
