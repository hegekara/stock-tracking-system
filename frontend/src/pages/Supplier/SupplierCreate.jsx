import React, { useEffect, useState } from "react";
import { Form, Button, Spinner, Alert } from "react-bootstrap";
import { apiFetch } from "../../api";
import Header from "../../components/Header";

const SupplierCreate = () => {
    const [addresses, setAddresses] = useState([]);
    const [loading, setLoading] = useState(true);
    const [form, setForm] = useState({
        name: "",
        contactName: "",
        phone: "",
        email: "",
        addressId: "",
        cityName: "",
        districtName: "",
        isNewAddress: false
    });
    const [message, setMessage] = useState(null);

    const fetchAddresses = async () => {
        const res = await apiFetch("/api/adress/list");
        if (res.ok) {
            setAddresses(res.data);
        }
        setLoading(false);
    };

    useEffect(() => {
        fetchAddresses();
    }, []);

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value
        });
    };

    const handleCheckboxChange = () => {
        setForm({
            ...form,
            isNewAddress: !form.isNewAddress,
            addressId: ""
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage(null);

        let finalAddressId = form.addressId;

        if (form.isNewAddress) {
            const addressRes = await apiFetch("/api/adress", {
                method: "POST",
                body: {
                    cityName: form.cityName,
                    districtName: form.districtName,
                    deleted: false
                }
            });

            if (!addressRes.ok) {
                setMessage({ type: "danger", text: "Adres eklenemedi." });
                return;
            }
            finalAddressId = addressRes.data.id;
        }

        const supplierRes = await apiFetch("/api/Supplier", {
            method: "POST",
            body: {
                name: form.name,
                contactName: form.contactName,
                phone: form.phone,
                email: form.email,
                addressId: parseInt(finalAddressId, 10)
            }
        });

        if (supplierRes.ok) {
            setMessage({ type: "success", text: "Tedarikçi başarıyla eklendi." });
            setForm({
                name: "",
                contactName: "",
                phone: "",
                email: "",
                addressId: "",
                cityName: "",
                districtName: "",
                isNewAddress: false
            });
        } else {
            setMessage({ type: "danger", text: "Tedarikçi eklenemedi." });
        }
    };

    if (loading) return <Spinner animation="border" className="mt-4" />;

    return (
        <>
            <Header />
            <div className="container mt-4 px-5">
                <h2>Tedarikçi Ekle</h2>
                {message && <Alert variant={message.type}>{message.text}</Alert>}
                <Form onSubmit={handleSubmit}>
                    <Form.Group className="mb-3">
                        <Form.Label>Firma Adı</Form.Label>
                        <Form.Control
                            type="text"
                            name="name"
                            value={form.name}
                            onChange={handleChange}
                            required
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>İlgili Kişi</Form.Label>
                        <Form.Control
                            type="text"
                            name="contactName"
                            value={form.contactName}
                            onChange={handleChange}
                            required
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Telefon</Form.Label>
                        <Form.Control
                            type="text"
                            name="phone"
                            value={form.phone}
                            onChange={handleChange}
                            required
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Email</Form.Label>
                        <Form.Control
                            type="email"
                            name="email"
                            value={form.email}
                            onChange={handleChange}
                            required
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Check
                            type="checkbox"
                            label="Yeni Adres Ekle"
                            checked={form.isNewAddress}
                            onChange={handleCheckboxChange}
                        />
                    </Form.Group>

                    {!form.isNewAddress && (
                        <Form.Group className="mb-3">
                            <Form.Label>Adres Seç</Form.Label>
                            <Form.Select
                                name="addressId"
                                value={form.addressId}
                                onChange={handleChange}
                                required
                            >
                                <option value="">Seçiniz...</option>
                                {addresses.map((a) => (
                                    <option key={a.id} value={a.id}>
                                        {a.cityName} - {a.districtName}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                    )}

                    {form.isNewAddress && (
                        <>
                            <Form.Group className="mb-3">
                                <Form.Label>Şehir</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="cityName"
                                    value={form.cityName}
                                    onChange={handleChange}
                                    required
                                />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>İlçe</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="districtName"
                                    value={form.districtName}
                                    onChange={handleChange}
                                    required
                                />
                            </Form.Group>
                        </>
                    )}

                    <Button variant="primary" type="submit">
                        Kaydet
                    </Button>
                </Form>
            </div>
        </>
    );
};

export default SupplierCreate;
