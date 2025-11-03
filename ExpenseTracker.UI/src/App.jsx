import React from 'react';
import {BrowserRouter, Routes, Route, Navigate} from 'react-router-dom';
import Home from './pages/Home.jsx';
import Login from './pages/Login.jsx';
import Unauthorized from './pages/Unauthorize.jsx';
import ProtectedRoute, { getUserIdFromToken } from './components/ProtectedRoute.jsx';
//Admin Imports
import AdminDashboard from './pages/Admin/Dashboard.jsx';
import CategoriesMgmt from './pages/Admin/CategoriesMgmt.jsx';
import UserMgmt from './pages/Admin/UserMgmt.jsx';
//User Imports
import UserDashboard from './pages/User/Dashboard.jsx';
import ExpenseMgmt from './pages/User/ExpenseMgmt.jsx';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/home" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/unauthorized" element={<Unauthorized />} />

        <Route
          path="/admin/dashboard"
          element={
            <ProtectedRoute allowedRole="Admin">
              <AdminDashboard />
            </ProtectedRoute>
          }
        />

        <Route
          path="/admin/categories"
          element={
            <ProtectedRoute allowedRole="Admin">
            <CategoriesMgmt />
            </ProtectedRoute>
          }
        />

        <Route
          path="/admin/users"
          element={
            <ProtectedRoute allowedRole="Admin">
              <UserMgmt />
            </ProtectedRoute>
          }
        />

        <Route
          path="/user/dashboard"
          element={
            <ProtectedRoute allowedRole="User">
              <UserDashboard />
            </ProtectedRoute>
          }
        />

        <Route
          path="/user/expenses"
          element={
            <ProtectedRoute allowedRole="User">
            <ExpenseMgmt />
            </ProtectedRoute>
            }
        />
      </Routes>

    </BrowserRouter>
  );
}