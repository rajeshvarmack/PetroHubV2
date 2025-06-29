# PetroHub - Petrol Station Management System

A comprehensive .NET 9 Web API solution for managing petrol station operations, built using Clean Architecture principles.

## 🏗️ Project Structure

```
PetroHub/
├── src/
│   ├── PetroHub.Domain/          # Core business entities and domain logic
│   │   ├── Common/              # Base entities, interfaces, result patterns
│   │   ├── ValueObjects/        # Money, Address, PhoneNumber
│   │   ├── Interfaces/          # Repository and UnitOfWork contracts
│   │   └── Exceptions/          # Domain-specific exceptions
│   │
│   ├── PetroHub.Application/     # Business logic and use cases
│   │   ├── Common/              # Pagination, base DTOs
│   │   ├── DTOs/                # Data Transfer Objects
│   │   ├── Services/            # Business logic implementation
│   │   ├── Interfaces/          # Service contracts
│   │   ├── Extensions/          # Mapping extensions
│   │   └── Resources/           # Localization resources (EN, AR, HI)
│   │
│   ├── PetroHub.Infrastructure/ # Data access and external services
│   │   ├── Data/                # Entity Framework DbContext
│   │   ├── Repositories/        # Repository implementations
│   │   └── Extensions/          # Service registration
│   │
│   └── PetroHub.API/            # Web API layer
│       ├── Controllers/         # API controllers
│       ├── Middleware/          # Global exception handling, localization
│       ├── Extensions/          # Service configuration
│       └── Resources/           # API-specific resources
└── PetroHub.sln
```

## 🚀 Features

### ✅ Completed Infrastructure
- **Clean Architecture** - Proper separation of concerns with dependency inversion
- **Entity Framework Core 9** - Latest ORM with SQL Server support
- **Multi-language Support** - English (default), Arabic, Hindi with resource files
- **Global Exception Handling** - Centralized error management with proper HTTP status codes
- **Repository Pattern** - Generic repository with Unit of Work
- **Result Pattern** - Consistent error handling without exceptions
- **Soft Delete** - Logical deletion support with automatic query filtering
- **Audit Trail** - Automatic tracking of creation/modification dates
- **Swagger Documentation** - Interactive API documentation
- **Health Checks** - Application and database health monitoring
- **CORS Support** - Cross-origin resource sharing configuration
- **Validation** - FluentValidation integration
- **Logging** - Built-in .NET logging framework

### 🏗️ Architecture Patterns
- **Clean Architecture** - Independent, testable, and maintainable layers
- **SOLID Principles** - Well-structured, extensible code
- **Repository Pattern** - Data access abstraction
- **Unit of Work** - Transaction management
- **Value Objects** - Immutable domain objects (Money, Address, PhoneNumber)
- **Base Service Pattern** - Reusable service implementation

## 🛠️ Technology Stack

- **.NET 9** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core 9** - Object-Relational Mapping
- **SQL Server** - Database (LocalDB for development)
- **FluentValidation** - Input validation library
- **Swagger/OpenAPI** - API documentation
- **Microsoft Localization** - Multi-language support

## 📋 Prerequisites

- .NET 9 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 / VS Code / JetBrains Rider

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd PetroHubV2
```

### 2. Restore Packages
```bash
dotnet restore
```

### 3. Update Database Connection
Update the connection string in `src/PetroHub.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PetroHubDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4. Build the Solution
```bash
dotnet build
```

### 5. Run the Application
```bash
cd src/PetroHub.API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5220`
- Swagger UI: `http://localhost:5220/swagger`

## 📊 API Endpoints

### Health Checks
- `GET /health` - Application health status
- `GET /health/ready` - Readiness probe
- `GET /health/live` - Liveness probe

### API Documentation
- `GET /swagger` - Interactive API documentation

## 🌐 Multi-Language Support

The application supports multiple languages through resource files:

- **English (default)**: `SharedResources.resx`
- **Arabic**: `SharedResources.ar.resx`
- **Hindi**: `SharedResources.hi.resx`

To add a new language, create a new resource file following the pattern `SharedResources.{culture}.resx`.

Language is determined by the `Accept-Language` HTTP header.

## 🏗️ Development Guidelines

### Adding New Features
1. **Domain First** - Add entities, value objects, and domain logic in the Domain layer
2. **Application Services** - Implement business logic in the Application layer
3. **Infrastructure** - Add repository implementations and configurations
4. **API Controllers** - Create RESTful endpoints in the API layer

### Best Practices Implemented
- Async/await throughout the application
- Proper error handling with Result pattern
- Input validation with FluentValidation
- Automatic audit trails
- Soft delete functionality
- Generic repository pattern
- Dependency injection
- Clean separation of concerns

## 🔧 Configuration

### Application Settings
Key configuration sections in `appsettings.json`:
- `ConnectionStrings` - Database connections
- `Logging` - Logging levels and providers
- `Cors` - Cross-origin resource sharing settings
- `ApiSettings` - API metadata for Swagger

### Environment-Specific Settings
- `appsettings.Development.json` - Development environment
- `appsettings.Production.json` - Production environment (create as needed)

## 📝 Next Steps

The foundation is complete and ready for feature development. You can now add:

1. **Domain Entities** - Customer, Product, Sale, Pump, Tank, Staff, etc.
2. **Business Logic** - Sales processing, inventory management, reporting
3. **Authentication & Authorization** - JWT, role-based access control
4. **Specific Controllers** - Customer management, sales, inventory, etc.
5. **Database Migrations** - Entity configurations and initial data

## 🤝 Contributing

1. Follow Clean Architecture principles
2. Write tests for new features
3. Update documentation
4. Follow coding standards
5. Use meaningful commit messages

---

**Built with ❤️ using Clean Architecture and .NET 9**
