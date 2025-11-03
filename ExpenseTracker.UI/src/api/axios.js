import axios from 'axios';

const instance = axios.create({
  // Prefer VITE_API_BASE_URL but fall back to localhost API during development
  baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:7249/api',
})

// Ensure we send JSON by default for API requests
instance.defaults.headers.common['Content-Type'] = 'application/json';

instance.interceptors.request.use((config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
    // The stored token may already include the 'Bearer ' prefix depending on the server.
    // Normalize it here so we don't end up with 'Bearer Bearer ...' which can cause 401s.
    config.headers.Authorization = token.startsWith('Bearer ') ? token : `Bearer ${token}`;
    }
    return config;
});

instance.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401 || error.response?.status === 403) {
      localStorage.removeItem('authToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default instance;