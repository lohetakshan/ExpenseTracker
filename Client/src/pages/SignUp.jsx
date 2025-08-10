import React, { useState } from "react";
const apiUrl = import.meta.env.VITE_API_BASE_URL;

const SignUp = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const handleRegister = async (e) => {
    e.preventDefault();

    const user = { username, passwordHash: password }; // Assuming password is hashed on the server

    try {
      const response = await fetch(`${apiUrl}/users`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(user),
      });

      if (response.ok) {
        setMessage("User registered successfully!");
        setUsername("");
        setPassword("");
      } else {
        setMessage("Registration failed.");
      }
    } catch (error) {
      setMessage("Error connecting to server.");
    }
  };

  return (
    <div style={{ padding: "2rem" }}>
      <h2>Sign up</h2>
      <form onSubmit={handleRegister}>
        <div>
          <label>Username:</label>
          <input
            type="text"
            id="username"
            name="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            id="password"
            name="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Register</button>
      </form>
      <p>{message}</p>
    </div>
  );
};

export default SignUp;