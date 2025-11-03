import { useEffect, useState } from 'react';
import UserNavBar from '../../components/User/NavBar';
import './Dashboard.css';
import api from '../../api/axios';
import { getUserIdFromToken } from '../../components/ProtectedRoute.jsx';

export default function Dashboard() {
  const [expenses, setExpenses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [editingExpense, setEditingExpense] = useState(null);

  const totalAmount = expenses.reduce((sum, e) => sum + (Number(e.amount) || 0), 0);

  useEffect(() => {
    const token = localStorage.getItem('authToken');
    if (!token) return;

    const userId = getUserIdFromToken(token);
    if (userId) {
      fetchExpenses(userId);
    }
  }, []);

  async function fetchExpenses(userId) {
    try {
      const response = await api.get('/Expense', {
        params: { UserId: userId }
      });
      setExpenses(response.data);
    } catch (err) {
      console.error('Failed to fetch expenses:', err);
      setError('Unable to load expenses.');
    } finally {
      setLoading(false);
    }
  }

  async function handleDelete(expenseId) {
    const userId = getUserIdFromToken(localStorage.getItem('authToken'));
    if (!userId) return;

    try {
      await api.delete(`/Expense/${expenseId}`, {
        params: { UserId: userId }
      });
      setExpenses(prev => prev.filter(e => e.expenseId !== expenseId));
    } catch (err) {
      console.error('Failed to delete expense:', err);
      alert('Delete failed.');
    }
  }

  async function handleUpdate(expenseId, updatedFields) {
    const userId = getUserIdFromToken(localStorage.getItem('authToken'));
    if (!userId) return;

    try {
      await api.put(`/Expense/${expenseId}`, updatedFields, {
        params: { UserId: userId }
      });
      fetchExpenses(userId);
      setEditingExpense(null);
    } catch (err) {
      console.error('Failed to update expense:', err);
      alert('Update failed.');
    }
  }

  return (
    <div className="user-dashboard">
      <UserNavBar />
      <div className="user-header">
        <h2>Your Expenses</h2>
        <div className="total-amount">Total: ₹{totalAmount.toFixed(2)}</div>
      </div>

      {loading ? (
        <p>Loading expenses...</p>
      ) : error ? (
        <p className="error-message">{error}</p>
      ) : expenses.length === 0 ? (
        <p>No expenses found.</p>
      ) : (
        <table className="expense-table">
          <thead>
            <tr>
              <th>Date</th>
              <th>Title</th>
              <th>Amount</th>
              <th>Category</th>
              <th>Notes</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {expenses.map((exp) => (
              <tr key={exp.expenseId}>
                <td>{new Date(exp.date).toLocaleDateString()}</td>
                <td>{exp.title}</td>
                <td>₹{exp.amount.toFixed(2)}</td>
                <td>{exp.categoryName || 'Uncategorized'}</td>
                <td>{exp.notes}</td>
                <td>
                  <button
                    className="edit-btn"
                    onClick={() => setEditingExpense(exp)}
                    title="Edit"
                  >
                    ✏️
                  </button>
                  <button
                    className="delete-btn"
                    onClick={() => handleDelete(exp.expenseId)}
                    title="Delete"
                  >
                    ➖
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {editingExpense && (
        <div className="edit-modal">
          <h3>Edit Expense</h3>
          <input
            type="text"
            value={editingExpense.title}
            onChange={(e) =>
              setEditingExpense({ ...editingExpense, title: e.target.value })
            }
          />
          <input
            type="number"
            value={editingExpense.amount}
            onChange={(e) =>
              setEditingExpense({ ...editingExpense, amount: e.target.value })
            }
          />
          <textarea
            value={editingExpense.notes}
            onChange={(e) =>
              setEditingExpense({ ...editingExpense, notes: e.target.value })
            }
          />
          <div className="modal-actions">
            <button
              onClick={() =>
                handleUpdate(editingExpense.expenseId, {
                  title: editingExpense.title,
                  amount: editingExpense.amount,
                  date: editingExpense.date,
                  notes: editingExpense.notes,
                  categoryId: editingExpense.categoryId
                })
              }
            >
              Save
            </button>
            <button onClick={() => setEditingExpense(null)}>Cancel</button>
          </div>
        </div>
      )}
    </div>
  );
}