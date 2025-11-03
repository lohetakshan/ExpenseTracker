import { Navigate } from 'react-router-dom';

// Small helper to decode a JWT payload without external dependency.
// This avoids ESM default-export issues with some bundlers/packages.
function decodeJwt(token) {
  try {
    if (!token) return null;
    const t = token.startsWith('Bearer ') ? token.slice(7) : token;
    const base64Url = t.split('.')[1];
    if (!base64Url) return null;
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch {
    return null;
  }
}

function getRoleFromToken(token) {
  const payload = decodeJwt(token);
  if (!payload) return null;
  const candidates = [
    'RoleClaimType',
    'role',
    'roles',
    'Role',
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role'
  ];
  for (const key of candidates) {
    if (Object.prototype.hasOwnProperty.call(payload, key)) {
      const v = payload[key];
      return Array.isArray(v) ? v[0] : v;
    }
  }
  return null;
}

function getUserIdFromToken(token) {
  const payload = decodeJwt(token);
  if (!payload) return null;

  const candidates = [
    'nameidentifier', // Common in ASP.NET Identity
    'sub',            // Standard JWT subject
    'id',             // Custom claim
    'userId',         // Alternate custom claim
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
  ];

  for (const key of candidates) {
    if (Object.prototype.hasOwnProperty.call(payload, key)) {
      return payload[key];
    }
  }

  return null;
}

function ProtectedRoute({ children, allowedRole }) {
  const token = localStorage.getItem('authToken');
  if (!token) return <Navigate to="/login" />;

  const role = getRoleFromToken(token);
  // If there is no explicit role claim, allow access to user routes
  // when a valid user id/subject exists in the token. This helps
  // with tokens that don't include a role but are valid user tokens.
  if (!role) {
    if (allowedRole && String(allowedRole).toLowerCase() === 'user') {
      const userId = getUserIdFromToken(token);
      if (userId) return children;
    }
    return <Navigate to="/unauthorized" />;
  }

  if (String(role).toLowerCase() !== String(allowedRole).toLowerCase()) {
    return <Navigate to="/unauthorized" />;
  }

  return children;
}

// ✅ Export all together
export {
  ProtectedRoute as default,
  getRoleFromToken,
  getUserIdFromToken
};