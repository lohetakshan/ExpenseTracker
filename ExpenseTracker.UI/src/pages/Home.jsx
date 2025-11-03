import './Home.css';
import { useNavigate } from 'react-router-dom';

export default function Home() {
  const navigate = useNavigate();

  return (
    <div className="home-container">
      <div className="home-content">
        <h1>Welcome to Expense Tracker</h1>
        <p>Track your expenses. Control your future.</p>
        <button onClick={() => navigate('/login')}>Get Started</button>
      </div>
    </div>
  );
}