# Sales Territory Management System

A full-stack web application for managing sales territories with a modern .NET 8 Web API backend and React frontend. This project demonstrates enterprise-level software engineering practices, clean architecture patterns, and production-ready features.

## 🏗️ Architecture Overview

This is a **full-stack application** consisting of:

- **Backend**: .NET 8 Web API with Entity Framework Core and PostgreSQL
- **Frontend**: React 19 with Vite, modern JavaScript, and responsive UI
- **Database**: PostgreSQL with JSON support for flexible demographics storage
- **Deployment**: Production-ready with structured logging, error handling, and CORS

## 🚀 Key Features

### Backend (.NET 8 Web API)
- **Clean Architecture** - Proper separation of concerns with Controllers, Services, Repositories
- **SOLID Principles** - Interface segregation, dependency injection, single responsibility
- **Entity Framework Core** - Repository pattern with async/await, database migrations
- **FluentValidation** - Comprehensive input validation with custom business rules
- **Global Exception Handling** - Centralized error management with custom middleware
- **DTOs & ViewModels** - Proper API design with data transfer optimization
- **Service Layer** - Business logic abstraction and encapsulation
- **Structured Logging** - Serilog integration with contextual information
- **CORS Configuration** - Secure cross-origin resource sharing
- **Swagger/OpenAPI** - Interactive API documentation

### Frontend (React 19)
- **Modern React Patterns** - Hooks, functional components, component composition
- **Error Boundaries** - Graceful error handling and user experience
- **State Management** - Local state with useState and useEffect
- **API Integration** - RESTful API consumption with error handling
- **Responsive Design** - Mobile-friendly UI with CSS Grid and Flexbox
- **Form Validation** - Client-side and server-side validation integration
- **Modal Components** - Reusable modal patterns for forms and details
- **Loading States** - User feedback during async operations
- **Network Error Handling** - Graceful degradation and retry mechanisms

## 🛠️ Technology Stack

### Backend
- .NET 8
- Entity Framework Core
- PostgreSQL
- FluentValidation
- Serilog
- Swagger/OpenAPI
- CORS

### Frontend
- React 19
- Vite (Build Tool)
- Modern JavaScript (ES6+)
- CSS3 with Flexbox/Grid
- ESLint (Code Quality)

## 📋 API Endpoints

### Business Endpoints
- `GET /api/Territories` - Get all territories
- `GET /api/Territories/{id}` - Get territory by ID
- `POST /api/Territories` - Create new territory
- `PUT /api/Territories/{id}` - Update territory
- `DELETE /api/Territories/{id}` - Delete territory

### Health Check Endpoints
- `GET /health` - Basic application health check
- `GET /health/ready` - Readiness check (app ready to serve traffic)
- `GET /health/live` - Liveness check (app is alive)

## 🎯 Design Patterns & Best Practices

### Backend Patterns
- **Repository Pattern** - Data access abstraction and testability
- **Service Layer Pattern** - Business logic encapsulation
- **DTO Pattern** - Data transfer optimization and API design
- **Dependency Injection** - Loose coupling and testability
- **Global Exception Handling** - Centralized error management
- **Extension Methods** - Clean Program.cs and service registration
- **Middleware Pattern** - Request/response pipeline customization

### Frontend Patterns
- **Component Composition** - Reusable, maintainable components
- **Error Boundary Pattern** - Graceful error handling
- **Custom Hooks** - Logic reuse and separation of concerns
- **Modal Pattern** - User interaction and data management
- **Form Handling** - Controlled components with validation
- **API Integration** - Async operations with error handling

## 🔧 Setup & Development

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- PostgreSQL

### Backend Setup
```bash
cd SalesTerritoryApi
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend Setup
```bash
cd salesterritoryui
npm install
npm run dev
```

### Access Points
- **API**: `https://localhost:7004/swagger`
- **Frontend**: `http://localhost:5173`
- **Health Checks**: `https://localhost:7004/health`

## 📊 Database Schema

- **SalesTerritory** - Main entity with Name, ZipCodes, Demographics
- **Migrations** - Version-controlled database changes
- **JSON Support** - Demographics stored as JSONB in PostgreSQL
- **Auto-generated IDs** - Database-generated primary keys

## 🔍 Validation & Error Handling

### Backend Validation
- Territory name: Required, max 100 characters
- Zip codes: Required, 5-digit format validation
- Demographics: JSON object validation
- Custom FluentValidation rules

### Frontend Validation
- Client-side form validation
- Server-side error integration
- Network error handling
- User-friendly error messages

## 🚀 Production Ready Features

- **Structured Logging** - Serilog with contextual information
- **Global Exception Handling** - Centralized error management
- **Input Validation** - Comprehensive validation at multiple layers
- **Database Migrations** - Version-controlled schema changes
- **Clean Architecture** - Maintainable and testable code structure
- **CORS Configuration** - Secure cross-origin requests
- **Error Boundaries** - Graceful frontend error handling
- **Responsive Design** - Mobile-friendly user interface

## 📁 Project Structure

```
TaskManagerApi/
├── SalesTerritoryApi/           # .NET 8 Web API Backend
│   ├── Controllers/             # API Controllers
│   ├── Data/                    # Entity Framework DbContext
│   ├── Models/                  # Domain Models & DTOs
│   ├── Services/                # Business Logic Layer
│   ├── Repositories/            # Data Access Layer
│   ├── Validators/              # FluentValidation Rules
│   ├── Middleware/              # Custom Middleware
│   └── Extensions/              # Service Registration
└── salesterritoryui/            # React Frontend
    ├── src/
    │   ├── App.jsx              # Main Application Component
    │   ├── TerritoryForm.jsx    # Form Component
    │   ├── TerritoryDetailsModal.jsx # Details Modal
    │   └── ErrorBoundary.jsx    # Error Boundary Component
    └── package.json             # Dependencies & Scripts
```

This project demonstrates **senior-level software engineering** skills including clean architecture, modern development practices, comprehensive error handling, and production-ready features suitable for enterprise applications.