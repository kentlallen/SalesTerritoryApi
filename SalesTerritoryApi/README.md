# Sales Territory API

A professional .NET 8 Web API for managing sales territories with Entity Framework Core and PostgreSQL.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
SalesTerritoryApi/
├── Controllers/           # API Controllers
├── Data/                  # Data Access Layer (DbContext, Migrations)
├── Models/               # Domain Models & DTOs
│   ├── DTOs/             # Data Transfer Objects
│   └── ViewModels/       # Response Models
├── Services/             # Business Logic Layer
│   └── Interfaces/       # Service Contracts
├── Repositories/         # Data Access Layer
├── Validators/           # FluentValidation Rules
├── Extensions/           # Extension Methods
├── Middleware/           # Custom Middleware
└── Configuration/        # App Settings
```

## 🚀 Features

- **Clean Architecture** - Proper separation of concerns
- **SOLID Principles** - Interface segregation, dependency injection
- **Entity Framework Core** - Repository pattern, migrations
- **FluentValidation** - Input validation with custom rules
- **Global Exception Handling** - Centralized error management
- **DTOs & ViewModels** - Proper API design
- **Service Layer** - Business logic abstraction
- **Extension Methods** - Clean Program.cs
- **PostgreSQL** - Production-ready database
- **Serilog** - Structured logging

## 🛠️ Technologies

- .NET 8
- Entity Framework Core
- PostgreSQL
- FluentValidation
- Serilog
- Swagger/OpenAPI

## 📋 API Endpoints

- `GET /api/Territories` - Get all territories
- `GET /api/Territories/{id}` - Get territory by ID
- `POST /api/Territories` - Create new territory
- `PUT /api/Territories/{id}` - Update territory
- `DELETE /api/Territories/{id}` - Delete territory

## 🔧 Setup

1. **Database Setup**
   ```bash
   # Update connection string in appsettings.json
   # Run migrations
   dotnet ef database update
   ```

2. **Run the Application**
   ```bash
   dotnet run
   ```

3. **Access Swagger UI**
   - Development: `https://localhost:7004/swagger`

## 🎯 Key Design Patterns

- **Repository Pattern** - Data access abstraction
- **Service Layer** - Business logic encapsulation
- **DTO Pattern** - Data transfer optimization
- **Dependency Injection** - Loose coupling
- **Global Exception Handling** - Centralized error management

## 📊 Database Schema

- **SalesTerritory** - Main entity with Name, ZipCodes, Demographics
- **Migrations** - Version-controlled database changes
- **JSON Support** - Demographics stored as JSON

## 🔍 Validation Rules

- Territory name: Required, max 100 characters
- Zip codes: Required, 5-digit format validation
- Demographics: JSON object validation

## 🚀 Production Ready

- Structured logging with Serilog
- Global exception handling
- Input validation
- Database migrations
- Clean architecture
- Professional folder structure
