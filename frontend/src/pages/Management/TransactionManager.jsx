import React, { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { apiFetch } from "../../api";
import Header from "../../components/Header";

const TransactionManager = () => {
    const [transactions, setTransactions] = useState([]);
    const [loading, setLoading] = useState(true);

    const getTransactions = async () => {
        try {
            const res = await apiFetch("/api/transaction");
            if (res.ok) {
                setTransactions(res.data);
                console.log(transactions);
                
            } else {
                console.error("API hatası:", res.status);
            }
        } catch (err) {
            console.error("Veri çekilirken hata oluştu:", err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        getTransactions();
    }, []);
    if (loading) {
        return <div className="text-center mt-4">Yükleniyor...</div>;
    }

    return (
        <>
            <Header />
            <div className="container mt-4">
                <h2>İşlem Listesi</h2>
                <table className="table table-striped table-bordered mt-3">
                    <thead className="table-dark">
                        <tr>
                            <th>ID</th>
                            <th>Tarih</th>
                            <th>Kategori</th>
                            <th>Tedarikçi</th>
                            <th>Ürün Adı</th>
                            <th>Açıklama</th>
                            <th>Miktar</th>
                            <th>İşlem Tipi</th>
                        </tr>
                    </thead>
                    <tbody>
                        {transactions.map((t) => (
                            <tr key={t.id}>
                                <td>{t.id}</td>
                                <td>{new Date(t.transactionDate).toLocaleString()}</td>
                                <th>{t.product.category.name} - {t.product.category.description}</th>
                                <th>{t.product.supplier.name}</th>
                                <td>{t.product?.name}</td>
                                <td>{t.product?.description}</td>
                                <td>{t.quantity}</td>
                                <td>
                                    {t.transactionType === 0 ? (
                                        <span className="badge bg-success">Artırma</span>
                                    ) : (
                                        <span className="badge bg-danger">Azaltma</span>
                                    )}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </>
    );
};

export default TransactionManager;
