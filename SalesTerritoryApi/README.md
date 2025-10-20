# Sales Territory API - .NET 8 Web API

A professional .NET 8 Web API for managing sales territories with enterprise-level architecture, comprehensive validation, and production-ready features. This API demonstrates senior-level .NET development skills with clean architecture patterns, robust error handling, and modern development practices.

## 🏗️ Clean Architecture

This project follows **Clean Architecture** principles with clear separation of concerns and dependency inversion:

```
SalesTerritoryApi/
├── Controllers/           # API Controllers (Presentation Layer)
├── Data/                  # Data Access Layer (DbContext, Migrations)
├── Models/               # Domain Models & DTOs
│   ├── DTOs/             # Data Transfer Objects
│   └── ViewModels/       # Response Models
├── Services/             # Business Logic Layer
│   └── Interfaces/       # Service Contracts (Abstractions)
├── Repositories/         # Data Access Layer
├── Validators/           # FluentValidation Rules
├── Extensions/           # Extension Methods for DI
├── Middleware/           # Custom Middleware
└── Program.cs            # Application Configuration
```

## 🚀 Enterprise Features

### Architecture & Design Patterns
- **Clean Architecture** - Proper separation of concerns with dependency inversion
- **SOLID Principles** - Interface segregation, dependency injection, single responsibility
- **Repository Pattern** - Data access abstraction and testability
- **Service Layer Pattern** - Business logic encapsulation and abstraction
- **DTO Pattern** - Data transfer optimization and API design
- **Dependency Injection** - Loose coupling and testability
- **Global Exception Handling** - Centralized error management with custom middleware
- **Extension Methods** - Clean Program.cs and service registration

### Data Access & Persistence
- **Entity Framework Core** - Modern ORM with async/await patterns
- **Repository Pattern** - Data access abstraction and testability
- **Database Migrations** - Version-controlled schema changes
- **PostgreSQL** - Production-ready database with JSON support
- **JSON Support** - Flexible demographics storage with JSONB

### Validation & Error Handling
- **FluentValidation** - Comprehensive input validation with custom business rules
- **Global Exception Handling** - Centralized error management with custom middleware
- **Model Validation** - Data annotations and custom validation rules
- **Error Response Formatting** - Consistent API error responses
- **Logging Integration** - Structured logging with Serilog

### Production-Ready Features
- **Structured Logging** - Serilog integration with contextual information
- **CORS Configuration** - Secure cross-origin resource sharing
- **Swagger/OpenAPI** - Interactive API documentation
- **Health Checks** - Application health monitoring with multiple endpoints
- **Configuration Management** - Environment-specific settings

## 🛠️ Technology Stack

- **.NET 8** - Latest .NET framework with modern features
- **Entity Framework Core** - Modern ORM with async/await
- **PostgreSQL** - Production-ready database with JSON support
- **FluentValidation** - Comprehensive validation framework
- **Serilog** - Structured logging with contextual information
- **Swagger/OpenAPI** - Interactive API documentation
- **CORS** - Cross-origin resource sharing

## 📋 API Endpoints

### Business Endpoints

| Method | Endpoint | Description | Response |
|--------|----------|-------------|----------|
| `GET` | `/api/Territories` | Get all territories | `200 OK` |
| `GET` | `/api/Territories/{id}` | Get territory by ID | `200 OK` / `404 Not Found` |
| `POST` | `/api/Territories` | Create new territory | `201 Created` / `400 Bad Request` |
| `PUT` | `/api/Territories/{id}` | Update territory | `200 OK` / `400 Bad Request` / `404 Not Found` |
| `DELETE` | `/api/Territories/{id}` | Delete territory | `204 No Content` / `404 Not Found` |

### Health Check Endpoints

| Method | Endpoint | Description | Response |
|--------|----------|-------------|----------|
| `GET` | `/health` | Basic application health check | `200 OK` / `503 Service Unavailable` |
| `GET` | `/health/ready` | Readiness check (app ready to serve traffic) | `200 OK` / `503 Service Unavailable` |
| `GET` | `/health/live` | Liveness check (app is alive) | `200 OK` |

## 🎯 Advanced Design Patterns

### 1. **Repository Pattern**
```csharp
public interface ITerritoryRepository
{
    Task<IEnumerable<SalesTerritory>> GetAllAsync();
    Task<SalesTerritory?> GetByIdAsync(int id);
    Task<SalesTerritory> CreateAsync(SalesTerritory territory);
    Task UpdateAsync(SalesTerritory territory);
    Task DeleteAsync(int id);
}
```

### 2. **Service Layer Pattern**
```csharp
public interface ITerritoryService
{
    Task<IEnumerable<TerritoryViewModel>> GetAllAsync();
    Task<TerritoryViewModel?> GetByIdAsync(int id);
    Task<TerritoryViewModel> CreateAsync(CreateTerritoryDto dto);
    Task<TerritoryViewModel?> UpdateAsync(int id, UpdateTerritoryDto dto);
    Task<bool> DeleteAsync(int id);
}
```

### 3. **DTO Pattern**
- **CreateTerritoryDto** - Input validation and data transfer
- **UpdateTerritoryDto** - Update operations with validation
- **TerritoryViewModel** - Response formatting and data shaping

### 4. **Dependency Injection**
- Constructor injection for all dependencies
- Interface-based abstractions
- Service lifetime management
- Extension methods for clean registration

### 5. **Global Exception Handling**
- Custom middleware for centralized error handling
- Consistent error response formatting
- Logging integration
- User-friendly error messages

## 🔧 Setup & Development

### Prerequisites
- .NET 8 SDK
- PostgreSQL
- Entity Framework Core CLI

### Database Setup
```bash
# Update connection string in appsettings.json
# Run migrations
dotnet ef database update
```

### Development
```bash
dotnet restore
dotnet run
```

### Access Points
- **API**: `https://localhost:7004/swagger`
- **Health Check**: `https://localhost:7004/health`
- **Readiness Check**: `https://localhost:7004/health/ready`
- **Liveness Check**: `https://localhost:7004/health/live`

## 📊 Database Schema

### SalesTerritory Entity
```csharp
public class SalesTerritory
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public List<string> ZipCodes { get; set; } = new();

    public Dictionary<string, object> Demographics { get; set; } = new();
}
```

### Key Features
- **Auto-generated IDs** - Database-generated primary keys
- **JSON Support** - Demographics stored as JSONB in PostgreSQL
- **Migrations** - Version-controlled database changes
- **Constraints** - Data integrity with validation

## 🔍 Validation & Business Rules

### FluentValidation Rules
- **Territory Name**: Required, max 100 characters, unique validation
- **Zip Codes**: Required, 5-digit format validation, no duplicates
- **Demographics**: JSON object validation, required fields

### Validation Features
- **Server-side Validation** - Comprehensive input validation
- **Custom Validators** - Business rule validation
- **Error Formatting** - User-friendly error messages
- **Field-level Errors** - Specific validation feedback

## 🚀 Production-Ready Features

### Health Monitoring
- **Basic Health Check** - `/health` endpoint for overall application health
- **Readiness Check** - `/health/ready` endpoint for traffic readiness
- **Liveness Check** - `/health/live` endpoint for application liveness
- **Container Integration** - Kubernetes and Docker health check support
- **Load Balancer Integration** - Traffic routing based on health status

### Logging & Monitoring
- **Structured Logging** - Serilog with contextual information
- **Request Logging** - HTTP request/response logging
- **Error Logging** - Comprehensive error tracking
- **Performance Logging** - Operation timing and metrics

### Security & Configuration
- **CORS Configuration** - Secure cross-origin requests
- **HTTPS Redirection** - Secure communication
- **Configuration Management** - Environment-specific settings
- **Health Checks** - Application monitoring

### Error Handling
- **Global Exception Handling** - Centralized error management
- **Custom Middleware** - Request/response pipeline customization
- **Error Response Formatting** - Consistent API responses
- **Logging Integration** - Error tracking and monitoring

## 🎯 Key Technical Highlights

### Modern .NET Features
- **Primary Constructors** - Clean controller definitions
- **Async/Await** - Non-blocking operations throughout
- **Nullable Reference Types** - Type safety and null handling
- **Pattern Matching** - Modern C# features

### Enterprise Patterns
- **Clean Architecture** - Maintainable and testable code
- **SOLID Principles** - Professional code organization
- **Dependency Injection** - Loose coupling and testability
- **Repository Pattern** - Data access abstraction
- **Service Layer** - Business logic encapsulation

This .NET 8 Web API demonstrates **senior-level backend development** skills including clean architecture, comprehensive validation, production-ready features, and enterprise-level error handling suitable for professional applications.
