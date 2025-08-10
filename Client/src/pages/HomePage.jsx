import React from "react";
import "./HomePage.css";
import { Link } from "react-router-dom";

const HomePage = () => {
  return (
    <div className="homepage">
      <h1 >Expense Tracker</h1>
      <p>Track your expenses, manage your budget, and stay in control.</p>
      <div style={{ marginTop: "1rem" }}>
        <Link to="/login">Old Bugga!</Link> | <Link to="/register">New Nigga!</Link>
      </div>
    </div>
  );
};

export default HomePage;