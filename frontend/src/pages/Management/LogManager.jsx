import React, { useEffect, useState } from "react";
import { apiFetch } from "../../api";
import Header from "../../components/Header";

function LogManager() {
    const [logs, setLogs] = useState([]);
    const [loading, setLoading] = useState(true);

    const fetchLogs = async () => {
        const res = await apiFetch("/api/logs/list");
        if (res.ok) {
            setLogs(res.data);
        } else {
            console.error("Log listesi alınamadı:", res);
        }
        setLoading(false);
    };

    useEffect(() => {
        fetchLogs();
    }, []);

    const downloadLog = async (fileName) => {
        try {
            const token = localStorage.getItem("token");
            const res = await fetch(`/api/logs/${fileName}`, {
                headers: {
                    Authorization: token ? `Bearer ${token}` : undefined,
                },
            });

            if (!res.ok) {
                console.error("Dosya indirilemedi:", res.statusText);
                return;
            }

            const blob = await res.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error("İndirme hatası:", error);
        }
    };

    return (
        <>
            <Header />
            <div className="container mt-4 px-5">
                <h2 className="text-2xl font-bold mb-4">Log Dosya Yöneticisi</h2>

                {loading ? (
                    <p>Yükleniyor...</p>
                ) : logs.length === 0 ? (
                    <p>Hiç log bulunamadı.</p>
                ) : (
                    <div className="bg-white shadow rounded p-4">
                        <table className="min-w-full table-auto">
                            <thead>
                                <tr className="bg-gray-200">
                                    <th className="px-4 py-2 text-left">Dosya Adı</th>
                                    <th className="px-4 py-2">İşlem</th>
                                </tr>
                            </thead>
                            <tbody>
                                {logs.map((log, index) => (
                                    <tr key={index} className="border-t">
                                        <td className="px-4 py-2">{log}</td>
                                        <td className="px-4 py-2 text-center">
                                            <button
                                                onClick={() => downloadLog(log)}
                                                className="bg-blue-500 hover:bg-blue-600 text-white px-3 py-1 rounded"
                                            >
                                                İndir
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>
        </>
    );
}

export default LogManager;
