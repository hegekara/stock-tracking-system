import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import { apiFetch } from '../../api';
import Header from '../../components/Header';

function UserCreate() {
    const [formData, setFormData] = useState({
        username: '',
        fullName: '',
        phoneNumber: '',
        email: '',
        password: ''
    });

    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await apiFetch("/api/auth/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: formData
            });

            if (response.status === 200) {
                navigate("/user-management");
            } else {
                const result = await response.json();
                setError(result.message || "Bir hata oluştu.");
            }
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <>
            <Header />
            <div className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
                <div className="container w-50">
                    <h2 className="text-center mb-4">Yeni Kullanıcı Oluştur</h2>
                    {error && <div className="alert alert-danger">{error}</div>}
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label className="form-label">Kullanıcı Adı</label>
                            <input type="text" className="form-control" name="username" value={formData.username} onChange={handleChange} required />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Ad Soyad</label>
                            <input type="text" className="form-control" name="fullName" value={formData.fullName} onChange={handleChange} required />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Telefon</label>
                            <input type="text" className="form-control" name="phoneNumber" value={formData.phoneNumber} onChange={handleChange} required />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Email</label>
                            <input type="email" className="form-control" name="email" value={formData.email} onChange={handleChange} required />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Şifre</label>
                            <input type="password" className="form-control" name="password" value={formData.password} onChange={handleChange} required />
                        </div>
                        <div className="d-flex justify-content-between">
                            <button type="submit" className="btn btn-success">Oluştur</button>
                            <button type="button" className="btn btn-secondary" onClick={() => navigate("/user-list")}>İptal</button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
}

export default UserCreate;
