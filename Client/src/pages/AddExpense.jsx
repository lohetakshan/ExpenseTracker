import React, { useState } from 'react';
import './AddExpense.css';

function AddExpense() {
  const apiUrl = import.meta.env.VITE_API_BASE_URL;

  const [formData, setFormData] = useState({
    title: '',
    amount: '',
    category: '',
    date: ''
  });

  const [statusMessage, setStatusMessage] = useState('');

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

  
 const expense = {
    ...formData,
    amount: Number(parseFloat(formData.amount).toFixed(2)) // ✅ Ensures it's a number, not a string
  };

console.log(typeof expense.amount); // should be 'number'

    try {
      const response = await fetch(`${apiUrl}/expenses`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(expense)
      });

      if (!response.ok) {
        throw new Error('Failed to save expense');
      }

      const result = await response.json();
      console.log('Saved:', result);
      setStatusMessage('Expense added successfully!');
      setFormData({ title: '', amount: '', category: '', date: '' });
    } catch (error) {
      console.error('Error:', error);
      setStatusMessage('Failed to add expense. Please try again.');
    }
  };

  return (
    <div className="container">
      <form className="add-expense-form" onSubmit={handleSubmit}>
        <h2>Add New Expense</h2>

        <div className="form-group">
          <label htmlFor="title">Title</label>
          <input
            type="text"
            id="title"
            name="title"
            value={formData.title}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="amount">Amount</label>
          <input
            type="number"
            id="amount"
            name="amount"
            value={formData.amount}
            onChange={handleChange}
            required
          />
        </div>

        {/* <div className="form-group">
          <label htmlFor="category">Category</label>
          <input
            type="text"
            id="category"
            name="category"
            value={formData.category}
            onChange={handleChange}
            required
          />
        </div> */}

        <div className="form-group">
  <label htmlFor="category">Category</label>
  <select
    id="category"
    name="category"
    value={formData.category}
    onChange={handleChange}
    required
  >
    <option value="">-- Select Category --</option>
    <option value="Groceries">Groceries</option>
    <option value="Utilities">Utilities</option>
    <option value="Travel">Travel</option>
    <option value="Food">Food</option>
    <option value="Entertainment">Entertainment</option>
    <option value="Health">Health</option>
    <option value="Other">Other</option>
  </select>
</div>


        <div className="form-group">
          <label htmlFor="date">Date</label>
          <input
            type="date"
            id="date"
            name="date"
            value={formData.date}
            onChange={handleChange}
            required
          />
        </div>

        <button type="submit">Add Expense</button>

        {statusMessage && <p className="status-message">{statusMessage}</p>}
      </form>
    </div>
  );
}

export default AddExpense;
