#!/bin/bash
set -e

echo "Initializing local environment..."

# 1. Setup local tools (ensures dotnet ef is available)
if [ ! -f ".config/dotnet-tools.json" ]; then
    echo "Creating tool manifest..."
    dotnet new tool-manifest
fi

echo "Installing/Verifying dotnet-ef tool..."
dotnet tool install dotnet-ef || true

# 2. Restore dependencies
echo "Restoring dependencies..."
dotnet restore

# 3. Update Database
echo "Applying database migrations..."
dotnet ef database update --project src/TaskManagement.Infrastructure --startup-project src/TaskManagement.API

# 4. Run Application
echo "Starting Task Management API..."
echo "You can access the API at http://localhost:5000/swagger (if in Dev) or use curl."
dotnet run --project src/TaskManagement.API
