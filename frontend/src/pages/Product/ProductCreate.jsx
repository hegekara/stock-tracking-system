import React, { useEffect, useState } from "react";
import { Form, Button, Card, Alert, Spinner, Row, Col } from "react-bootstrap";
import { apiFetch } from "../../api";
import Header from "../../components/Header";

const ProductCreate = () => {
    const [categories, setCategories] = useState([]);
    const [suppliers, setSuppliers] = useState([]);
    const [loading, setLoading] = useState(true);

    const [formData, setFormData] = useState({
        name: "",
        description: "",
        unitPrice: "",
        unitsInStock: "",
        categoryId: "",
        supplierId: "",
    });

    const [newCategory, setNewCategory] = useState({
        name: "",
        description: "",
        deleted: false,
    });

    const [useNewCategory, setUseNewCategory] = useState(false);

    const [message, setMessage] = useState(null);
    const [error, setError] = useState(null);

    const fetchData = async () => {
        try {
            const [catRes, supRes] = await Promise.all([
                apiFetch("/api/Category"),
                apiFetch("/api/Supplier"),
            ]);

            if (catRes.ok) setCategories(catRes.data);
            if (supRes.ok) setSuppliers(supRes.data);
        } catch (err) {
            setError("Veriler yüklenirken hata oluştu.");
        }
        setLoading(false);
    };

    useEffect(() => {
        fetchData();
    }, []);

    const handleChange = (e) => {
        setFormData((prev) => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    };

    const handleNewCategoryChange = (e) => {
        setNewCategory((prev) => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage(null);
        setError(null);

        let categoryId = formData.categoryId;

        if (useNewCategory) {
            const catRes = await apiFetch("/api/Category", {
                method: "POST",
                body: newCategory,
            });
            if (!catRes.ok) {
                setError("Kategori oluşturulurken hata oluştu.");
                return;
            }
            categoryId = catRes.data.id;
        }

        const body = {
            name: formData.name,
            description: formData.description,
            unitPrice: parseFloat(formData.unitPrice),
            unitsInStock: parseInt(formData.unitsInStock, 10),
            categoryId: parseInt(categoryId, 10),
            supplierId: parseInt(formData.supplierId, 10),
        };

        const res = await apiFetch("/api/Product", {
            method: "POST",
            body,
        });

        if (res.ok) {
            setMessage("Ürün başarıyla oluşturuldu!");
            setFormData({
                name: "",
                description: "",
                unitPrice: "",
                unitsInStock: "",
                categoryId: "",
                supplierId: "",
            });
            setNewCategory({
                name: "",
                description: "",
                deleted: false,
            });
            setUseNewCategory(false);
        } else {
            setError("Ürün oluşturulurken hata oluştu.");
        }
    };

    if (loading) return <Spinner animation="border" className="mt-4" />;

    return (
        <>
            <Header />
            <div className="container mt-4 px-5">
                <Card>
                    <Card.Header>
                        <h4>Ürün Oluştur</h4>
                    </Card.Header>
                    <Card.Body>
                        {message && <Alert variant="success">{message}</Alert>}
                        {error && <Alert variant="danger">{error}</Alert>}

                        <Form onSubmit={handleSubmit}>
                            <Form.Group className="mb-3">
                                <Form.Label>Ürün Adı</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="name"
                                    value={formData.name}
                                    onChange={handleChange}
                                    required
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Açıklama</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="description"
                                    value={formData.description}
                                    onChange={handleChange}
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Fiyat</Form.Label>
                                <Form.Control
                                    type="number"
                                    step="0.01"
                                    name="unitPrice"
                                    value={formData.unitPrice}
                                    onChange={handleChange}
                                    required
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Stok Adedi</Form.Label>
                                <Form.Control
                                    type="number"
                                    name="unitsInStock"
                                    value={formData.unitsInStock}
                                    onChange={handleChange}
                                    required
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Kategori</Form.Label>
                                <div className="mb-2">
                                    <Form.Check
                                        type="radio"
                                        label="Mevcut kategorilerden seç"
                                        name="categoryOption"
                                        checked={!useNewCategory}
                                        onChange={() => setUseNewCategory(false)}
                                    />
                                    <Form.Check
                                        type="radio"
                                        label="Yeni kategori oluştur"
                                        name="categoryOption"
                                        checked={useNewCategory}
                                        onChange={() => setUseNewCategory(true)}
                                    />
                                </div>

                                {!useNewCategory ? (
                                    <Form.Select
                                        name="categoryId"
                                        value={formData.categoryId}
                                        onChange={handleChange}
                                        required
                                    >
                                        <option value="">Seçiniz</option>
                                        {categories.map((cat) => (
                                            <option key={cat.id} value={cat.id}>
                                                {cat.name} - {cat.description}
                                            </option>
                                        ))}
                                    </Form.Select>
                                ) : (
                                    <>
                                        <Form.Control
                                            type="text"
                                            placeholder="Kategori adı"
                                            name="name"
                                            value={newCategory.name}
                                            onChange={handleNewCategoryChange}
                                            required
                                            className="mb-2"
                                        />
                                        <Form.Control
                                            type="text"
                                            placeholder="Kategori açıklaması"
                                            name="description"
                                            value={newCategory.description}
                                            onChange={handleNewCategoryChange}
                                            className="mb-2"
                                        />
                                    </>
                                )}
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Tedarikçi</Form.Label>
                                <Form.Select
                                    name="supplierId"
                                    value={formData.supplierId}
                                    onChange={handleChange}
                                    required
                                >
                                    <option value="">Seçiniz</option>
                                    {suppliers.map((sup) => (
                                        <option key={sup.id} value={sup.id}>
                                            {sup.name}
                                        </option>
                                    ))}
                                </Form.Select>
                            </Form.Group>

                            <Button type="submit" variant="primary">
                                Ürünü Oluştur
                            </Button>
                        </Form>
                    </Card.Body>
                </Card>
            </div>
        </>
    );
};

export default ProductCreate;
