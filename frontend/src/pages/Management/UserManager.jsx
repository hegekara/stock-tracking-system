import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { apiFetch } from '../../api';
import Header from '../../components/Header';
import { useNavigate } from 'react-router-dom';
import { FaUserShield } from 'react-icons/fa';
import { Modal, Form, Button } from 'react-bootstrap';

function UserManager() {
    const [users, setUsers] = useState([]);
    const [selectedUser, setSelectedUser] = useState(null);
    const [newRole, setNewRole] = useState('');
    const [showModal, setShowModal] = useState(false);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const fetchUsers = async () => {
        try {
            const response = await apiFetch("/api/user/list");
            if (response.ok) {
                setUsers(response.data);
            } else {
                throw new Error(`Hata kodu: ${response.status}`);
            }
        } catch (err) {
            console.error("Kullanıcılar alınırken hata oluştu:", err);
            setError(err.message);
        }
    };

    const handleDelete = async (id) => {
        try {
            const response = await apiFetch(`/api/user/delete/${id}`, {
                method: 'DELETE',
            });
            if (response.status === 200) {
                fetchUsers();
            } else if (response.status === 204) {
                setError("Müşteri Bulunamadı");
            } else {
                throw new Error(`Hata kodu: ${response.status}`);
            }
        } catch (err) {
            setError(err.message);
        }
    };

    const handleRoleChange = async () => {
        if (!selectedUser || !newRole) return;
        try {
            const response = await apiFetch(`/api/user/change-role/${selectedUser.id}`, {
                method: "PUT",
                body: { role: newRole } // apiFetch otomatik JSON.stringify yapıyor
            });
            if (response.ok) {
                setShowModal(false);
                fetchUsers();
            } else {
                throw new Error(`Rol değiştirme başarısız: ${response.status}`);
            }
        } catch (err) {
            setError(err.message);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    return (
        <>
            <Header />
            <div className="container mt-4">
                {error && <div className="alert alert-danger">{error}</div>}
                <div className="d-flex justify-content-between align-items-center mb-3">
                    <h2>Kullanıcı Listesi</h2>
                    <button className="btn btn-primary" onClick={() => navigate('/user-create')}>Kullanıcı Oluştur</button>
                </div>
                <table className="table table-bordered table-striped">
                    <thead className="table-dark">
                        <tr className="text-center">
                            <th>Kullanıcı Adı</th>
                            <th>Ad Soyad</th>
                            <th>Email</th>
                            <th>İşlemler</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.length === 0 ? (
                            <tr>
                                <td colSpan="6" className="text-center">Kullanıcı bulunamadı</td>
                            </tr>
                        ) : (
                            users.map(user => (
                                <tr key={user.id}>
                                    <td>{user.userName}</td>
                                    <td>{user.fullName}</td>
                                    <td>{user.email}</td>
                                    <td className="text-center">
                                        <button
                                            className="btn btn-sm btn-outline-danger me-2"
                                            onClick={() => {
                                                if (window.confirm("Bu kullanıcıyı silmek istediğinize emin misiniz?")) {
                                                    handleDelete(user.id);
                                                }
                                            }}
                                            title="Sil"
                                        >
                                            <i className="bi bi-trash"></i>
                                        </button>
                                        <FaUserShield
                                            style={{ cursor: 'pointer', color: 'blue' }}
                                            title="Rol Değiştir"
                                            onClick={() => { 
                                                setSelectedUser(user); 
                                                setNewRole('');
                                                setShowModal(true); 
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        )}
                    </tbody>
                </table>

                <Modal show={showModal} onHide={() => setShowModal(false)}>
                    <Modal.Header closeButton>
                        <Modal.Title>Rol Değiştir</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form.Select value={newRole} onChange={(e) => setNewRole(e.target.value)}>
                            <option value="">Rol Seçin</option>
                            <option value="Admin">Admin</option>
                            <option value="Manager">Manager</option>
                            <option value="User">User</option>
                        </Form.Select>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={() => setShowModal(false)}>İptal</Button>
                        <Button variant="primary" onClick={handleRoleChange}>Kaydet</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        </>
    );
}

export default UserManager;
