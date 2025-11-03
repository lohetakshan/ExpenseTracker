import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import './Login.css';

// Decode a JWT payload (safe for browser environment)
function parseJwt(token) {
  try {
    if (!token) return null;
    const t = token.startsWith('Bearer ') ? token.slice(7) : token;
    const base64Url = t.split('.')[1];
    if (!base64Url) return null;
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    return null;
  }
}

function getRoleFromPayload(payload) {
  if (!payload) return null;
  const candidates = [
    'RoleClaimType',
    'role',
    'roles',
    'Role',
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role'
  ];
  for (const key of candidates) {
    if (Object.prototype.hasOwnProperty.call(payload, key)) {
      const val = payload[key];
      if (Array.isArray(val)) return val[0];
      return val;
    }
  }
  return null;
}

export default function Login() {
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({ username: '', password: '' });
  const [error, setError] = useState('');
  const [showRegister, setShowRegister] = useState(false);
  // Use lowercase keys to match expected server payload (username, password, email, name)
  const [newUser, setNewUser] = useState({ username: '', passwordhash: '', email: '', name: '' });
  const [registerError, setRegisterError] = useState('');

  const handleChange = (e) => {
    setCredentials({ ...credentials, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const response = await api.post('/User/login', credentials);
      const token = response.data?.token;
      if (!token) {
        setError('Login succeeded but no token was returned by server.');
        return;
      }
      localStorage.setItem('authToken', token);
      const payload = parseJwt(token);
      const roleRaw = getRoleFromPayload(payload);
      let role = null;
      if (Array.isArray(roleRaw) && roleRaw.length) {
        role = String(roleRaw[0]).toLowerCase();
      } else if (typeof roleRaw === 'string') {
        role = roleRaw.toLowerCase();
      } else if (roleRaw && typeof roleRaw === 'object') {
        role = JSON.stringify(roleRaw).toLowerCase();
      }

      if (role === 'admin') {
        navigate('/admin/dashboard');
      } else if (role === 'user') {
        navigate('/user/dashboard');
      } else {
        const possibleUserId = payload?.nameidentifier || payload?.sub || payload?.id;
        if (possibleUserId) {
          navigate('/user/dashboard');
        } else {
          navigate('/');
        }
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Login failed. Please try again.');
    }
  };

  const handleRegisterChange = (e) => {
    setNewUser({ ...newUser, [e.target.name]: e.target.value });
  };

  const handleRegisterSubmit = async (e) => {
    e.preventDefault();
    setRegisterError('');
    try {
      // Debug: log the outgoing payload and resolved URL so server-side validation issues are easier to diagnose
      try {
        // eslint-disable-next-line no-console
        console.log('Register payload:', newUser);
        // eslint-disable-next-line no-console
        console.log('POST URL:', (api.defaults && api.defaults.baseURL ? api.defaults.baseURL : '') + '/User');
      } catch (logErr) {
        // ignore logging errors
      }

      await api.post('/User', newUser);

      alert('Registration successful! You can now log in.');
      setShowRegister(false);
      setNewUser({ username: '', passwordhash: '', email: '', name: '' });
    } catch (err) {
      // Provide more detailed error output in console and UI to help pinpoint why the server returned 400
      // eslint-disable-next-line no-console
      console.error('Registration error (full):', err);
      // eslint-disable-next-line no-console
      console.error('Registration error response.data:', err?.response?.data);

      const serverMsg = err?.response?.data?.message || err?.response?.data || null;
      setRegisterError(serverMsg ? JSON.stringify(serverMsg) : 'Registration failed. Please try again.');
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>Login</h2>
        {error && <div className="error-message">{error}</div>}
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="username"
            placeholder="Username"
            value={credentials.username}
            onChange={handleChange}
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Password"
            value={credentials.password}
            onChange={handleChange}
            required
          />
          <button type="submit">Login</button>
        </form>

        <button onClick={() => setShowRegister(!showRegister)} style={{ marginTop: '1rem' }} className="register-form">
          I'm New
        </button>

        {showRegister && (
          <form onSubmit={handleRegisterSubmit} className="register-form">
            <h3>Register</h3>
            {registerError && <div className="error-message">{registerError}</div>}
            <input
              type="text"
              name="username"
              placeholder="User Name"
              value={newUser.username}
              onChange={handleRegisterChange}
              required
            />
            <input
              type="password"
              name="passwordHash"
              placeholder="PasswordHash"
              value={newUser.password}
              onChange={handleRegisterChange}
              required
            />
            <input
              type="email"
              name="email"
              placeholder="Email"
              value={newUser.email}
              onChange={handleRegisterChange}
              required
            />

            <input
              type="text"
              name="name"
              placeholder="Name"
              value={newUser.name}
              onChange={handleRegisterChange}
              required
            />
            <button type="submit">Register</button>
          </form>
        )}
      </div>
    </div>
  );
}