import { useNavigate } from 'react-router-dom';
import './NavBar.css';
import { getUserIdFromToken } from '../ProtectedRoute.jsx';
import { useEffect, useState } from 'react';
import api from '../../api/axios';

export default function UserNavBar() {
  const navigate = useNavigate();
  const [userName, setUserName] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem('authToken');
    if (!token) return;

    const userId = getUserIdFromToken(token);
    if (!userId) return;

    api.get('/User/me')
      .then(res => {
        // tolerate different response shapes: { name }, { username }, { userName }
        const name = res.data?.name ?? res.data?.username ?? res.data?.userName ?? '';
        if (name) {
          setUserName(name);
        } else {
          console.warn('User name not found in response:', res.data);
        }
      })
      .catch(err => {
        console.error('Failed to fetch user info:', err);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('authToken');
    navigate('/login');
  };

  return (
    <nav className="user-nav">
      <div className="user-greeting">
        {!loading && userName && (
          <span>👋 Welcome, <strong>{userName}</strong></span>
        )}
      </div>
      <div className="user-actions">
        <button onClick={() => navigate('/user/expenses')}>➕ Add Expense</button>
        <button onClick={handleLogout}>🚪 Logout</button>
      </div>
    </nav>
  );
}