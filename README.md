# Employee Management System (EMS)
**ASP.NET Core MVC · SQL Server · Bootstrap 5 · jQuery**

A web-based system for managing employees and departments, with bulk upload support and a live dashboard.

---

## Features

| Module | Description |
|--------|-------------|
| Dashboard | Total employees, active departments, average salary |
| Departments | Add / Edit / Delete with validation |
| Employees | Add / Edit / Delete via popup modal (AJAX) |
| Search & Filter | Filter by name and/or department |
| Bulk Upload | Upload employees from CSV or Excel |

---

## Quick Start

**Prerequisites:** macOS, Docker Desktop, DBeaver, .NET SDK 10+

For full setup instructions see **SETUP.md**.

```bash
# Clone and navigate to project
cd EmployeeManagementSystem

# Restore packages and run
dotnet restore
dotnet run
```

Make sure the SQL Server Docker container is running before starting the app:
```bash
docker start sqlserver2019
```

---

## Tech Stack

- **Backend:** ASP.NET Core MVC, Entity Framework Core 8, C#
- **Database:** SQL Server 2019 (Docker)
- **Frontend:** Bootstrap 5, jQuery, AJAX
- **File Upload:** EPPlus (CSV + Excel)

---

## Assumptions

1. No authentication — out of scope per project specification
2. Hard delete for employees; department delete blocked if employees are assigned
3. Departments are auto-created during bulk upload if they don't exist
4. JoiningDate is optional
5. EPPlus used under NonCommercial license for assessment purposes