import { useNavigate } from 'react-router-dom';
import './NavBar.css';

export default function AdminNavBar() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('authToken');
    navigate('/login');
  };

  return (
    <nav style={{ display: 'flex', justifyContent: 'space-between', padding: '1rem', background: '#eee' }}>
      <button onClick={() => navigate('/admin/categories')}>Manage Categories</button>
      <button onClick={handleLogout}>Logout</button>
    </nav>
  );
}