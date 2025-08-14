import { createRoot } from 'react-dom/client';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

import Home from './pages/Home';
import Login from './pages/Auth/Login';
import ProtectedRoute from './components/ProtectedRoute';
import ProductList from './pages/Product/ProductList';
import StockTransaction from './pages/Product/StockTransaction';
import ProductCreate from './pages/Product/ProductCreate';

createRoot(document.getElementById('root')).render(
  <BrowserRouter>
    <Routes>
      <Route path="/login" element={<Login />} />



      <Route path="/" element={
        <ProtectedRoute>
          <Home />
        </ProtectedRoute>
      } />

      <Route path="/home" element={
        <ProtectedRoute>
          <Home />
        </ProtectedRoute>
      } />



      <Route path="/product-list" element={
        <ProtectedRoute>
          <ProductList />
        </ProtectedRoute>
      } />
      <Route path="/stock-transaction" element={
        <ProtectedRoute>
          <StockTransaction />
        </ProtectedRoute>
      } />
      <Route path="/product-create" element={
        <ProtectedRoute>
          <ProductCreate />
        </ProtectedRoute>
      } />



    </Routes>
  </BrowserRouter>
);
