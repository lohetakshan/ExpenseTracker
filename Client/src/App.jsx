import { useEffect, useState } from 'react';
import './App.css';
import './global.css';
import './components/Navbar.css';
import { Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage';
import SignUp from './pages/SignUp';
import SignIn from './pages/SignIn';
import Dashboard from './pages/Dashboard';
import AddExpense from './pages/AddExpense';
import NavBar from './components/Navbar';

function App() {
    return (
        <>
        <NavBar />
        <div className="container">
        <Routes>
            <Route path="/" element={<HomePage/>}/>
            <Route path="/register" element={<SignUp/>}/>
            <Route path="/login" element={<SignIn/>}/>
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/add" element={<AddExpense />} />
        </Routes>
        </div>
        </>
    );
}

export default App;