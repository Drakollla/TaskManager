# Task Manager API

RESTful API for managing tasks, built with .NET 8 and following Clean Architecture principles. This project demonstrates modern backend development practices, including CQRS pattern implementation via MediatR.

# Key Features

- **Clean Architecture:** Strict separation of concerns (Domain, Repository, Application, API).
- **CQRS Pattern:** Implemented using **MediatR** library to decouple command and query responsibilities.
- **Advanced Data Access:**
  - **Entity Framework Core** with Code First approach.
  - Complex relationships: One-to-Many (Categories) and Many-to-Many (Tags).
  - Efficient querying with **Filtering**, **Searching**, and **Sorting**.
  - **Pagination** implementation (returning metadata in `X-Pagination` header).
- **Validation & Error Handling:**
  - **FluentValidation** integrated into MediatR Pipeline Behavior.
  - **Global Exception Handling Middleware** for consistent error responses.
- **DTO Mapping:** Static mapper classes (no external dependencies).

Tools: Swagger/OpenAPI for documentation and testing.

# Architecture Overview

The solution is divided into six projects:

1.  **Domain:** Contains Entities, Enums, Exceptions, and Request Features (Pagination/Filtering parameters). Has no dependencies.
2.  **Repository (Infrastructure):** Implements Data Access Logic, DB Context, and EF Core Configurations.
3.  **Application:** Contains Business Logic, CQRS Handlers (Commands/Queries), DTOs, Validators.
4.  **Shared:** Contains DTOs and static mappers.
5.  **TaskManagerAPI:** Presentation layer (Controllers) and Middleware configuration.
6.  **Tests:** Unit tests with xUnit and Moq.

# Installation

1.  Clone the repository:
    ```bash
    git clone https://github.com/Drakollla/TaskManager.git
    ```
2.  Set the `SECRET` environment variable (required for JWT):
    ```bash
    # Windows
    set SECRET=your-secret-key-min-32-chars
    ```
3.  Update the connection string in `appsettings.json` if necessary.
4.  Apply migrations to create the database:
    ```bash
    Update-Database
    ```
    *(Run this command in Package Manager Console, selecting **Repository** as the default project).*
5.  Run the application:
    ```bash
    dotnet run --project TaskManagerAPI
    ```

# Optional: Seed Data

To populate the database with initial test data (Categories, Tags, Sample Tasks), uncomment the seeding line in `Program.cs` before running the application:

```csharp
// await app.SeedDataAsync();
```

# API Endpoints

The API includes endpoints for managing:

- **Tasks** (CRUD, Filter by Date, Search by Title, Pagination)
- **Categories**
- **Tags**

You can explore and test all endpoints via the integrated **Swagger UI** at `https://localhost:port/swagger`.
