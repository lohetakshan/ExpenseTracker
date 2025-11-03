import { useEffect, useState } from 'react';
import api from '../../api/axios';
import AdminNavBar from '../../components/Admin/NavBar';

export default function CategoriesMgmt() {
  const [categories, setCategories] = useState([]);
  const [newCategory, setNewCategory] = useState('');

  useEffect(() => {
    fetchCategories();
  }, []);

  async function fetchCategories() {
    try {
      const response = await api.get('/Category');
      setCategories(response.data);
    } catch (err) {
      console.error('Failed to fetch categories:', err);
    }
  }

  async function handleAdd() {
    if (!newCategory.trim()) return;
    try {
      await api.post('/Category', { name: newCategory });
      setNewCategory('');
      fetchCategories();
    } catch (err) {
      console.error('Failed to add category:', err);
    }
  }

  return (
    <div>
      <AdminNavBar />
      <div style={{ padding: '1rem' }}>
        <h2>Categories</h2>
        <input
          type="text"
          placeholder="New category"
          value={newCategory}
          onChange={(e) => setNewCategory(e.target.value)}
        />
        <button onClick={handleAdd}>Add</button>
        <ul>
          {categories.map((cat) => (
            <li key={cat.id}>{cat.name}</li>
          ))}
        </ul>
      </div>
    </div>
  );
}