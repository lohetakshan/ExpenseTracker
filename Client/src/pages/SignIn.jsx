import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const apiUrl = import.meta.env.VITE_API_BASE_URL;

//nlocalStorage.setItem("user", JSON.stringify(data)); // in SignIn.js

const SignIn = () => {
const [username, setUsername] = useState("");
const [password, setPassword] = useState("");
const [message, setMessage] = useState("");
const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch(`${apiUrl}/users/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, passwordhash: password }),
      });

      if (response.ok) {
        const data = await response.json();
        setMessage("Login successful!");
        // Optionally store token or user info
        // localStorage.setItem("user", JSON.stringify(data));
        navigate("/dashboard"); // 👈 Redirect to dashboard
      } else {
        setMessage("Invalid username or password.");
      }
    } catch (error) {
      setMessage("Error connecting to server.");
    }
  };

  return (
    <div style={{ padding: "2rem" }}>
      <h2>Sign In</h2>
      <form onSubmit={handleLogin}>
        <div>
          <label>Username:</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Login</button>
      </form>
      <p>{message}</p>
    </div>
  );
};

export default SignIn;