import { useEffect, useState } from 'react';
import api from '../../api/axios';
import { useNavigate } from 'react-router-dom';
import { getUserIdFromToken } from '../../components/ProtectedRoute.jsx';
import './ExpenseMgmt.css';


export default function ExpenseMgmt() {
  // Only keep category data and the form — we do not fetch the user's expenses here.
  const [categories, setCategories

  ] = useState([]);
  const [form, setForm] = useState({ date: '', title: '', amount: '', categoryId: '', notes: '' });
  const [submitLoading, setSubmitLoading] = useState(false);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    fetchCategories();
  }, []);

  async function fetchCategories() {
    try {
      const resp = await api.get('/Category');
      setCategories(resp.data || []);
    } catch (err) {
      console.warn('Failed to fetch categories, falling back to empty list', err);
      setCategories([]);
    }
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setSubmitLoading(true);
    setError('');
    try {
      const selectedCategoryName = (categories.find(c => String(c.id) === String(form.categoryId)) || {}).name || null;
      const payload = {
        date: form.date,
        title: form.title,
        amount: Number(form.amount) || 0,
        categoryId: form.categoryId || null,
        //categoryName: selectedCategoryName,
        notes: form.notes
      };

      // Attach userId explicitly if the API requires it (safe to include)
      const token = localStorage.getItem('authToken');
      const userId = getUserIdFromToken(token);
      if (userId) payload.userId = userId;

      //const resp = await api.post('/Expense', payload);
      const resp = await api.post('/Expense', payload, {
  params: { UserId: userId }
});
      // log server validation error body when available for easier debugging
      if (resp?.status >= 400) {
        console.error('Server returned non-success status', resp);
      }
  // clear form
  setForm({ date: '', title: '', amount: '', categoryId: '', notes: '' });
  // navigate back to dashboard as requested
  alert('Expense added successfully!');
  navigate('/user/dashboard');
    } catch (err) {
      console.error('Failed to add expense:', err, err.response?.data || err.message);
      setError('Unable to add expense. Please try again.');
    } finally {
      setSubmitLoading(false);
    }
  }

  return (
    <div className="expense-mgmt-root">
      <div className="expense-mgmt-inner">
        <h2>Add Expenses</h2>

        <form className="expense-form" onSubmit={handleSubmit}>
          <label>
            Date
            <input type="date" value={form.date} onChange={e => setForm({ ...form, date: e.target.value })} required />
          </label>

          <label>
            Title
            <input type="text" value={form.title} onChange={e => setForm({ ...form, title: e.target.value })} placeholder="Lunch, Uber etc." required />
          </label>

          <label>
            Amount
            <input type="number" step="0.01" value={form.amount} onChange={e => setForm({ ...form, amount: e.target.value })} required />
          </label>

          <label>
            Category
            <select value={form.categoryId} onChange={e => setForm({ ...form, categoryId: e.target.value })}>
              <option value="">-- Select category --</option>
              {categories.map((c, idx) => {
                // Support several possible id/name shapes returned by different APIs
                const cid = c?.id ?? c?.Id ?? c?._id ?? c?.categoryId ?? idx;
                const label = c?.name ?? c?.Name ?? c?.categoryName ?? String(cid);
                return (
                  <option key={cid} value={cid}>{label}</option>
                );
              })}
            </select>
          </label>

          <label>
            Notes
            <input type="text" value={form.notes} onChange={e => setForm({ ...form, notes: e.target.value })} />
          </label>

          <div className="expense-form-actions">
            <button type="submit" disabled={submitLoading}>{submitLoading ? 'Adding...' : 'Add Expense'}</button>
            <button type="button" onClick={() => navigate('/user/dashboard')}>Back to Dashboard</button>
          </div>

          {error && <p className="form-error">{error}</p>}
        </form>

        {/* Expense list removed — this page only handles adding a new expense and then navigates back to the dashboard. */}
      </div>
    </div>
  );
}