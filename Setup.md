# Employee Management System (EMS)
## ASP.NET Core MVC | SQL Server | Bootstrap

---

## Prerequisites

- macOS (Apple Silicon or Intel)
- DBeaver installed
- Internet connection

Everything else is installed in the steps below.

---

## Step 1: Install Docker Desktop

1. Download Docker Desktop from https://www.docker.com/products/docker-desktop/
2. Open the `.dmg` file and install
3. Launch Docker Desktop and accept the terms
4. Wait for Docker to finish starting (whale icon in menu bar goes steady)

---

## Step 2: Run SQL Server in Docker

Open Terminal and run:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sqlserver2019 \
   -d mcr.microsoft.com/mssql/server:2019-latest
```

> ⚠️ Use `MSSQL_SA_PASSWORD` — not `SA_PASSWORD`. The shorter name is incorrect and the container will silently fail.

> **Apple Silicon (M1/M2/M3):** You will see a platform warning about `linux/amd64` vs `linux/arm64`. This is expected and normal — SQL Server runs via Rosetta 2 emulation and works fine.

Verify the container is running:
```bash
docker ps
# Should show sqlserver2019 with port 0.0.0.0:1433->1433/tcp
```

Then wait 20–30 seconds and check the logs:
```bash
docker logs sqlserver2019
# Look for: "SQL Server is now ready for client connections."
```

> ⚠️ The container showing "Up" does NOT mean SQL Server is ready. Always wait for that log line before trying to connect.

**Managing the container (day-to-day):**
```bash
docker start sqlserver2019    # start after a Mac restart
docker stop sqlserver2019     # stop when done
docker ps                     # check status
```

---

## Step 3: Connect DBeaver to SQL Server

1. Open DBeaver → **New Database Connection** → select **SQL Server**
2. Enter these connection details:
   - **Host:** `localhost`
   - **Port:** `1433`
   - **Database:** `master`
   - **Authentication:** SQL Server Authentication
   - **Username:** `sa`
   - **Password:** `YourStrong@Passw0rd`
3. In the **Driver Properties** tab, set `trustServerCertificate` = `true`
4. Click **Test Connection** — if successful, click **Finish**

> If you get "Connection refused" — SQL Server isn't ready yet. Wait longer and check the logs from Step 2.

---

## Step 4: Create the Database

1. In DBeaver, right-click your connection → **SQL Editor → New SQL Script**
2. Run:
```sql
CREATE DATABASE EmployeeManagementDB;
```
3. Right-click the connection → **Refresh** → you should see `EmployeeManagementDB`

> ⚠️ Do NOT use `GO` in DBeaver — it is SSMS-only and will cause an error. The database is still created successfully but the error message is confusing. Just use a semicolon.

---

## Step 5: Install .NET SDK

```bash
brew install --cask dotnet-sdk
```

Verify:
```bash
dotnet --version
# Should print something like: 10.0.103
```

If `dotnet` is not found after install, add it to your PATH:
```bash
export PATH="$PATH:/usr/local/share/dotnet"
```
Then restart your terminal.

---

## Step 6: Create the Project

Navigate to where you want to save the project:
```bash
cd /your/preferred/folder
```

Create the ASP.NET Core MVC project:
```bash
dotnet new mvc -n EmployeeManagementSystem
cd EmployeeManagementSystem
```

> ⚠️ Never use spaces in the project name. `Employee Management System` will cause namespace errors (`namespace '_'`). Always use `PascalCase` with no spaces.

Immediately verify the `.csproj` file is correct:
```bash
cat EmployeeManagementSystem.csproj
```

You should see `<TargetFramework>net10.0</TargetFramework>` and **no** `<RootNamespace>_</RootNamespace>` line. If you see the underscore namespace, delete the project and recreate it — it is faster than trying to fix it.

---

## Step 7: Create Required Folders

```bash
mkdir Data
mkdir Services
```

---

## Step 8: Configure the Connection String

Open `appsettings.json` and replace its contents with:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EmployeeManagementDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;Encrypt=False;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> **Important macOS notes:**
> - Use a **comma** between host and port: `localhost,1433` (not a colon)
> - `TrustServerCertificate=True` is required for Docker's self-signed certificate
> - `Encrypt=False` helps avoid connection issues on macOS

---

## Step 9: Install NuGet Packages

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package EPPlus --version 7.0.0
```

Install the EF CLI tool globally (needed for migrations):
```bash
dotnet tool install --global dotnet-ef
```

Verify packages installed:
```bash
dotnet restore
dotnet build
# Should say: Build succeeded.
```

---

## Step 10: Test Run the Application

```bash
dotnet run
```

You should see:
```
Now listening on: http://localhost:5099
```

Open that URL in your browser — you should see the default ASP.NET welcome page.

Stop with **Ctrl + C**.

> The `warn: No XML encryptor configured` message is normal in development — ignore it.

---

## Step 11: Set Up the Database Tables

Once all source files are in place, run EF migrations to create the tables:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Then verify in DBeaver (refresh the connection) — you should see three tables under `EmployeeManagementDB`:
- `Departments`
- `Employees`
- `__EFMigrationsHistory`

Alternatively, run the included `DatabaseScript.sql` directly in DBeaver instead of using EF migrations.

---

## Step 12: Final Verification Checklist

```
✅ Docker Desktop running
✅ SQL Server container running (docker ps shows sqlserver2019)
✅ DBeaver connected to SQL Server
✅ EmployeeManagementDB database exists
✅ .NET SDK installed (dotnet --version works)
✅ Project created with no namespace errors
✅ appsettings.json has the connection string
✅ All NuGet packages installed (dotnet restore succeeds)
✅ dotnet build succeeds
✅ dotnet run shows the welcome page
✅ Tables created in DBeaver
```

---

## Features

| Module | Description |
|--------|-------------|
| Dashboard | Live stats — total employees, active departments, average salary |
| Departments | Add / Edit / Delete with duplicate name validation |
| Employees | Add / Edit / Delete via popup modal (AJAX, no page reload) |
| Search & Filter | Filter employees by name and/or department |
| Bulk Upload | Upload employees from CSV or Excel with row-level validation |

---

## Bulk Upload File Format

Use the included `sample_employees.csv` as a template:

```
Name,Email,Salary,Department Name,Joining Date
John Doe,john@example.com,50000,IT,2024-01-15
Jane Smith,jane@example.com,60000,HR,2024-02-01
```

- Supported formats: `.csv` and `.xlsx`
- If a department does not exist, it is **created automatically**
- Rows with invalid data are skipped and reported on the results page

---

## Assumptions

1. **No authentication.** Login and role-based access are out of scope per the project specification.
2. **Hard delete for employees.** Deleting an employee permanently removes the record.
3. **Department delete is guarded.** Deletion is blocked if employees are assigned to the department.
4. **Departments auto-created on bulk upload.** A department referenced in an upload file that doesn't exist is created automatically as active.
5. **SQL Server only.** Configured exclusively for SQL Server as specified — no PostgreSQL or SQLite.
6. **EPPlus NonCommercial license.** Used under the non-commercial license for educational/assessment purposes.
7. **JoiningDate is optional.** Employees can be saved without a joining date.
8. **macOS development environment.** SQL Server runs via Docker with Rosetta 2 emulation on Apple Silicon. Runs identically on Windows with a native SQL Server instance.

---

## Troubleshooting

| Problem | Solution |
|---------|----------|
| `Connection refused` in DBeaver | SQL Server not ready yet — wait 30 sec, check `docker logs sqlserver2019` |
| `Could not find stored procedure 'GO'` in DBeaver | Normal — ignore it, or just remove `GO`. Operation succeeded. |
| `namespace '_'` build errors | Project was created with spaces in name — recreate with `dotnet new mvc -n EmployeeManagementSystem` |
| `dotnet` command not found | Run `export PATH="$PATH:/usr/local/share/dotnet"` and restart terminal |
| `dotnet ef` command not found | Run `dotnet tool install --global dotnet-ef` |
| HTTPS certificate warning in browser | Run `dotnet dev-certs https --trust` |
| Docker platform warning on Apple Silicon | Expected — ignore it, SQL Server works via emulation |

---

## Project Structure

```
EmployeeManagementSystem/
├── Controllers/          — Request handlers (Home, Department, Employee)
├── Data/                 — ApplicationDbContext (EF Core)
├── Models/               — Department, Employee, DashboardViewModel, UploadResult
├── Services/             — FileUploadService (CSV + Excel processing)
├── Views/                — Razor views (.cshtml) per controller
├── wwwroot/              — Bootstrap, jQuery, site CSS/JS
├── Migrations/           — EF-generated migration files
├── appsettings.json      — Connection string configuration
├── Program.cs            — App entry point and service registration
├── DatabaseScript.sql    — Manual database creation script
└── sample_employees.csv  — Sample file for bulk upload testing
```

---

*Built with ASP.NET Core MVC · Entity Framework Core 8 · SQL Server 2019 · Bootstrap 5 · jQuery*