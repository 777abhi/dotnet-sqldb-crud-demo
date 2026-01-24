# Migration Guide

To initialize the database, follow these steps:

1.  **Create Initial Migration:**
    ```bash
    dotnet ef migrations add InitialCreate --project src/TaskManagement.Infrastructure --startup-project src/TaskManagement.API
    ```

2.  **Apply Migration (Update Database):**
    ```bash
    dotnet ef database update --project src/TaskManagement.Infrastructure --startup-project src/TaskManagement.API
    ```

3.  **Run the Application:**
    ```bash
    dotnet run --project src/TaskManagement.API
    ```
    The application will seed initial categories ("Work", "Personal", "Urgent") and a sample task on first run.

## Notes
*   **Database:** The solution uses SQLite. The database file `tasks.db` will be created in the API project directory.
*   **Seeding:** Data seeding happens automatically on application startup if the database exists and can be connected to.
