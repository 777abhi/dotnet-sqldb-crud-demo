# Task Management API

A .NET 8 Web API demonstrating CRUD operations, Entity Framework Core features, and Clean Architecture principles.

## 🚀 Features

- **Clean Architecture**: Organized into `API`, `Domain`, and `Infrastructure` layers.
- **Entity Framework Core 8**:
  - **SQLite Database**: Lightweight and easy to run locally.
  - **Shadow Properties**: Automatic tracking of `CreatedDate` and `UpdatedDate`.
  - **Global Query Filters**: Automatically filter out archived tasks (`!IsArchived`).
  - **Data Seeding**: Initial data population for easy testing.
- **Swagger UI**: Interactive API documentation.

## 🛠️ Tech Stack

- .NET 8
- Entity Framework Core 8
- SQLite

## 📂 Project Structure

- `src/TaskManagement.API`: The entry point of the application, containing Controllers and configuration.
- `src/TaskManagement.Domain`: Contains the core business logic, Entities (`WorkTask`, `Category`), and Enums.
- `src/TaskManagement.Infrastructure`: Handles database context (`TaskContext`), migrations, and data access.

## 🏃‍♂️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Running the Application

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd dotnet-sqldb-crud-demo
   ```

2. **Run the API:**
   You can run the application using the dotnet CLI:
   ```bash
   dotnet run --project src/TaskManagement.API
   ```
   Alternatively, you can use the provided helper script which handles tool installation and database updates:
   ```bash
   ./run_locally.sh
   ```

   The application will start, and the database will be automatically created and seeded.

3. **Access Swagger UI:**
   Open your browser and navigate to `http://localhost:5273/swagger` to explore the API endpoints.

### Database Migrations

If you make changes to the domain model, you can create and apply migrations using the following commands:

```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project src/TaskManagement.Infrastructure --startup-project src/TaskManagement.API

# Update the database
dotnet ef database update --project src/TaskManagement.Infrastructure --startup-project src/TaskManagement.API
```

## 📝 Key Concepts

### Shadow Properties
The application uses EF Core Shadow Properties to track creation and modification times without cluttering the domain entities. These are automatically updated in `TaskContext.SaveChanges`.

### Global Query Filters
A global query filter is applied to `WorkTask` entities to exclude archived tasks by default.

```csharp
modelBuilder.Entity<WorkTask>().HasQueryFilter(e => !e.IsArchived);
```

To include archived tasks in a query, use `.IgnoreQueryFilters()`.

## 📄 License

This project is licensed under the MIT License.
