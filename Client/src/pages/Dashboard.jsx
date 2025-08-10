import React from 'react';
import './Dashboard.css';

function Dashboard() {
  const expenses = [
    { icon: '🛒', title: 'Groceries', amount: 50 },
    { icon: '💡', title: 'Utilities', amount: 100 },
    { icon: '🚗', title: 'Travel', amount: 150 },
    { icon: '🍽️', title: 'Food', amount: 200 },
    { icon: '🎬', title: 'Entertainment', amount: 250 },
    { icon: '💊', title: 'Health', amount: 300 },
    { icon: '❓', title: 'Other', amount: 350 }

  ];

  return (
    <div className="dashboard">
      <h1 className="dashboard-title">Expense Dashboard</h1>
      <div className="card-grid">
        {expenses.map((expense, index) => (
          <div className="card" key={index}>
            <div className="card-icon">{expense.icon}</div>
            <div className="card-title">{expense.title}</div>
            <div className="card-amount">₹{expense.amount}</div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Dashboard;
