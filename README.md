# ExpenseTracker

A full-stack expense tracking application built with ASP.NET Core backend and React frontend.

## 📁 Project Structure
ExpenseTracker/ 
- Core/ - Domain models and business logic
- API/ - Web API endpoints
- Infrastructure/ - Data access and external services
- UI/ - React frontend (independent project)
- ExpenseTracker.sln - .NET Core solution file (backend only)

## 🚀 Tech Stack

- Backend: ASP.NET Core (.NET 8)
- Frontend: React + Vite
- Database: Azure SQL (planned)
- Hosting: Azure App Service + Azure Static Web Apps

## 🛠️ Setup Instructions

### Backend
bash
- Open ExpenseTracker.sln in Visual Studio
- Restore NuGet packages and build

### Frontend
- cd UI
- npm install
- npm run dev

## 📦 Deployment
- Backend: Publish via Visual Studio to Azure App Service
- Frontend: Deploy UI/dist to Azure Static Web Apps

## 🔐 Notes
- .env files are excluded via .gitignore
- Line endings normalized for cross-platform use

## What functions?
* User registration, login & logout.
* Expense dashboard, addition, modification & deletion.
