import "./Dashboard.css";
import AdminNavBar from "../../components/Admin/NavBar";

export default function Dashboard() {
  return (
    <div className="admin-dashboard">
      <AdminNavBar />
      <h2>Registered Users</h2>
      <UserMgmt />
    </div>
  );
}