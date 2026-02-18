# Employee Management System - Module 0
## What We Actually Did - Session Log
**Date:** February 17, 2026  
**Duration:** ~3 hours  
**Status:** ✅ Module 0 Complete

---

## 🎯 Starting Point

**Your Setup:**
- MacBook (Apple Silicon - M1/M2/M3)
- DBeaver already installed
- Some .NET SDK already installed
- No project yet

**Goal:** Set up complete development environment for ASP.NET MVC with SQL Server

---

## 📝 WHAT WE ACTUALLY DID - STEP BY STEP

### Step 1: Installed Docker Desktop ✅

**What you did:**
1. Downloaded Docker Desktop for macOS
2. Installed it
3. Launched Docker Desktop

**No issues here** - smooth installation

---

### Step 2: Created SQL Server Container ✅

**First attempt - had an issue:**
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sqlserver2019 \
   -d mcr.microsoft.com/mssql/server:2019-latest
```

**Problem:** Container installed but didn't show in `docker ps`

**Solution:** You clicked "Play" button in Docker Desktop UI, then ran `docker ps` again

**Result:**
```
CONTAINER ID   IMAGE                                        PORTS                    NAMES
e61263256862   mcr.microsoft.com/mssql/server:2019-latest   0.0.0.0:1433->1433/tcp   sqlserver2019
```

**Note:** Got platform warning - this is EXPECTED on Apple Silicon:
```
WARNING: The requested image's platform (linux/amd64) does not match 
the detected host platform (linux/arm64/v8)
```
We ignored it - SQL Server runs fine via emulation.

**Second attempt - correct command:**
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sqlserver2019 \
   -d mcr.microsoft.com/mssql/server:2019-latest
```

**Key change:** `MSSQL_SA_PASSWORD` instead of `SA_PASSWORD` (correct environment variable)

---

### Step 3: Connected DBeaver to SQL Server ✅

**Initial Problem:** "Connection refused" error

**Why:** SQL Server takes 20-30 seconds to fully start even after container shows "Up"

**Solution:** 
1. Waited 30 seconds
2. Checked logs: `docker logs sqlserver2019`
3. Waited for "SQL Server is now ready for client connections"
4. Then tried connecting in DBeaver

**Connection details used:**
- Host: `localhost`
- Port: `1433`
- Username: `sa`
- Password: `YourStrong@Passw0rd`
- Driver Property: `trustServerCertificate=true`

**Result:** Connection successful! ✅

---

### Step 4: Created Database in DBeaver ✅

**What you ran:**
```sql
CREATE DATABASE EmployeeManagementDB;
GO
```

**Issue Encountered:** DBeaver error:
```
SQL Error [2812] [S0062]: Could not find stored procedure 'GO'.
```

**But:** Database was actually created successfully!

**Lesson Learned:** 
- `GO` is SSMS-specific command
- DBeaver doesn't recognize it
- Just use semicolon `;` in DBeaver
- Error message is misleading - database was created

**Correct command for DBeaver:**
```sql
CREATE DATABASE EmployeeManagementDB;
```

**Verified:** Refreshed connection in DBeaver → saw "EmployeeManagementDB" ✅

---

### Step 5: Attempted to Create ASP.NET Project - Multiple Issues 🔴

#### Attempt 1: Project with SPACES in name ❌

**What happened:**
Created project folder: "Employee Management System" (with spaces)

**Problem:**
```
error CS0234: The type or namespace name 'Models' does not exist in the namespace '_'
```

**Why:** C# doesn't allow spaces in namespaces, so .NET converted it to underscore `_`

**Lesson:** NEVER use spaces in project names!

---

#### Attempt 2: Fixed space issue, but .csproj was broken ❌

**What we found:**
```bash
ls -la | grep csproj
-rw-r--r--   1 alihafeez  staff   935 Feb 17 20:41 ..csproj
```

**Problem:** File was named `..csproj` instead of `EmployeeManagementSystem.csproj`!

**Also found:** When we checked the content:
```xml
<RootNamespace>_</RootNamespace>
```

The namespace was set to underscore!

**Tried to fix:** Changed RootNamespace to `EmployeeManagementSystem`, but still had errors because source files were already generated with `_` namespace.

---

#### Attempt 3: Created fresh project ✅

**Decision:** Easier to recreate than fix all broken files

**Removed old project:**
```bash
cd /Volumes/Office/Assessments/PTA
mv EmployeeManagementSystem EmployeeManagementSystem_OLD
```

**Created fresh project:**
```bash
dotnet new mvc -n EmployeeManagementSystem
cd EmployeeManagementSystem
```

**Checked .csproj immediately:**
```bash
cat EmployeeManagementSystem.csproj
```

**This time it was clean:**
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

**No `<RootNamespace>_</RootNamespace>`** - perfect!

---

### Step 6: Installed NuGet Packages ✅

**Commands we ran:**
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package EPPlus
```

**Note:** We're using .NET 10.0, so packages installed version 8.0.0 (latest stable compatible)

**Verified:**
```bash
dotnet restore
dotnet build
```

Both succeeded! ✅

---

### Step 7: Created Folders ✅

**Commands:**
```bash
mkdir Data
mkdir Services
mkdir Docs
```

**Purpose:**
- `Data/` - Will hold ApplicationDbContext.cs
- `Services/` - Will hold FileUploadService.cs
- `Docs/` - Documentation files

---

### Step 8: Configured appsettings.json ✅

**Original content (missing connection string):**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Updated to:**
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

**Key settings for macOS/Docker:**
- `TrustServerCertificate=True` - Required for Docker's self-signed certificate
- `Encrypt=False` - Helps with macOS connection issues

---

### Step 9: First Successful Run! 🎉

**Command:**
```bash
dotnet run
```

**Output:**
```
Using launch settings from /Volumes/Office/Assessments/PTA/EmployeeManagementSystem/Properties/launchSettings.json...
Building...
warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
      No XML encryptor configured. Key {fedad072-cf20-43f0-84c2-2db5fec1b15e} may be persisted to storage in unencrypted form.
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5099
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**Warnings are normal for development!**

**Opened browser:** `http://localhost:5099` → Saw ASP.NET welcome page! ✅

---

### Step 10: Understanding Questions ❓

**You asked important architecture questions:**

1. **Why does app run without Data/ and Services/ folders?**
   - Answer: They're optional, just for organization
   - App only needs: Controllers, Views, Models, Program.cs

2. **Where is the routing engine?**
   - Answer: Built into ASP.NET, configured in Program.cs
   - `app.UseRouting()` enables it
   - `app.MapControllerRoute()` defines URL patterns

3. **What does `[ResponseCache(...)]` mean?**
   - Answer: Attribute telling browser "don't cache this page"
   - Used on Error page so users always see fresh errors

4. **Why `new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }`?**
   - Answer: Object initializer with null-coalescing
   - Tries Activity.Current?.Id first, falls back to TraceIdentifier
   - Ensures we always have an ID for tracking errors

5. **Why do some views have `@model` and some don't?**
   - Answer: Only views that receive data need `@model`
   - Static pages don't need it

6. **Why no `builder.Services.AddDbContext`?**
   - Answer: We haven't created database context yet
   - That's Module 1!

7. **Can't see .csproj file in VS Code?**
   - Answer: It's there! Just might be hidden
   - Can view with: `ls -la | grep csproj`

---

### Step 11: Git Setup ✅

**Initialized Git:**
```bash
git init
git remote add origin https://github.com/AliHafeez337/PTA-Assessment-Employee_Management_System.git
```

**Created .gitignore:**
```bash
cat > .gitignore << 'EOF'
## Build results
[Dd]ebug/
[Rr]elease/
[Bb]in/
[Oo]bj/

# User-specific files
*.suo
*.user

# NuGet Packages
*.nupkg
**/packages/*

# macOS
.DS_Store

# Backup folders
*_OLD/
EOF
```

**Committed and pushed:**
```bash
git add .
git commit -m "Module 0 Complete: Project setup with SQL Server, EF Core, and working application"
git push -u origin main --force
```

Used `--force` because remote had old broken files.

---

## 🐛 ISSUES WE ENCOUNTERED & HOW WE SOLVED THEM

### Issue 1: Platform Warning on Apple Silicon
**Error:**
```
WARNING: The requested image's platform (linux/amd64) does not match 
the detected host platform (linux/arm64/v8)
```

**Solution:** Ignored it! This is expected. SQL Server runs via emulation (Rosetta 2) and works perfectly.

---

### Issue 2: Connection Refused to SQL Server
**Error:** "TCP/IP connection to localhost port 1433 has failed"

**Cause:** SQL Server takes 20-30 seconds to start after container shows "Up"

**Solution:** 
1. Wait 30 seconds
2. Check logs: `docker logs sqlserver2019`
3. Look for: "SQL Server is now ready for client connections"
4. Then connect

---

### Issue 3: DBeaver "GO" Error
**Error:** "Could not find stored procedure 'GO'"

**Cause:** `GO` is SSMS-specific, not standard SQL

**Solution:** Don't use `GO` in DBeaver. Database was created successfully despite error.

---

### Issue 4: Project Name with Spaces
**Error:** `namespace '_'` errors everywhere

**Cause:** C# doesn't allow spaces in namespaces

**Solution:** Recreated project without spaces: `EmployeeManagementSystem`

---

### Issue 5: .csproj File Named Wrong
**Found:** `..csproj` instead of `EmployeeManagementSystem.csproj`

**Also:** `<RootNamespace>_</RootNamespace>` in the file

**Solution:** Recreated project fresh - faster than fixing

---

### Issue 6: ErrorViewModel Not Found
**Error:** Build errors about missing ErrorViewModel

**Cause:** Fresh template sometimes doesn't include this file

**Solution:** Would have created manually, but fresh project already had it

---

## 📊 FINAL WORKING CONFIGURATION

### System Details
- **OS:** macOS (Apple Silicon - ARM64)
- **IDE:** VS Code
- **.NET Version:** 10.0.103
- **Database:** SQL Server 2019 (Docker)
- **Database Tool:** DBeaver

### Project Location
```
/Volumes/Office/Assessments/PTA/EmployeeManagementSystem/
```

### Docker Container
```
Name: sqlserver2019
Port: 1433
Password: YourStrong@Passw0rd
Status: Running
```

### Database
```
Server: localhost:1433
Database: EmployeeManagementDB
Username: sa
Status: Created and accessible
```

### Application
```
URL: http://localhost:5099
Status: Running successfully
Framework: ASP.NET Core MVC (net10.0)
```

### Installed Packages
```
- Microsoft.EntityFrameworkCore.SqlServer (8.0.0)
- Microsoft.EntityFrameworkCore.Tools (8.0.0)
- Microsoft.EntityFrameworkCore.Design (8.0.0)
- EPPlus (8.4.2)
```

### Project Structure
```
EmployeeManagementSystem/
├── Controllers/
│   └── HomeController.cs
├── Data/              ← Created (empty)
├── Docs/              ← Created
├── Models/
│   └── ErrorViewModel.cs
├── Services/          ← Created (empty)
├── Views/
│   ├── Home/
│   └── Shared/
├── wwwroot/
├── Program.cs
├── appsettings.json   ← Updated with connection string
└── EmployeeManagementSystem.csproj
```

---

## 🎓 KEY LESSONS LEARNED

### 1. Naming Conventions Matter
- ✅ Use PascalCase: `EmployeeManagementSystem`
- ✅ Use hyphens: `employee-management-system`
- ❌ Never use spaces: `Employee Management System`

### 2. Docker on Apple Silicon
- Platform warnings are normal
- SQL Server runs via emulation
- Works perfectly for development
- Slightly slower than native (not noticeable)

### 3. SQL Server Startup Time
- Container "Up" ≠ SQL Server ready
- Wait 20-30 seconds after `docker ps` shows running
- Check logs: `docker logs sqlserver2019`
- Look for: "ready for client connections"

### 4. DBeaver vs SSMS
- DBeaver doesn't support `GO` command
- Use semicolons `;` instead
- Error about GO is misleading - operation usually succeeded

### 5. .csproj File is Critical
- Every .NET project MUST have it
- Check it immediately after project creation
- Make sure `<RootNamespace>` is correct
- If broken, recreate project (faster than fixing)

### 6. Connection String for Docker SQL Server
- Must include: `TrustServerCertificate=True`
- Should include: `Encrypt=False` (for macOS)
- Use comma for port: `localhost,1433` (not colon)

### 7. Starting Fresh vs Fixing
- Sometimes recreating is faster than debugging
- Keep backups before deleting
- Fresh start = clean slate = no hidden issues

---

## ⚙️ COMMANDS WE USED

### Docker Commands
```bash
# Create SQL Server container
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sqlserver2019 \
   -d mcr.microsoft.com/mssql/server:2019-latest

# Check status
docker ps

# View logs
docker logs sqlserver2019

# Start/stop container
docker start sqlserver2019
docker stop sqlserver2019
```

### Project Creation
```bash
# Navigate to location
cd /Volumes/Office/Assessments/PTA

# Create project (no spaces!)
dotnet new mvc -n EmployeeManagementSystem

# Enter project
cd EmployeeManagementSystem

# Open in VS Code
code .
```

### Package Installation
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package EPPlus
```

### Build and Run
```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Clean build artifacts
dotnet clean
```

### Folder Creation
```bash
mkdir Data
mkdir Services
mkdir Docs
```

### Git Commands
```bash
git init
git add .
git commit -m "Module 0 Complete"
git remote add origin https://github.com/AliHafeez337/PTA-Assessment-Employee_Management_System.git
git push -u origin main --force
```

---

## 📈 TIME BREAKDOWN

**Total Time:** ~3 hours

**Time Distribution:**
- Docker + SQL Server setup: 30 minutes
- DBeaver connection + database creation: 15 minutes
- First project attempt (with spaces): 20 minutes ❌
- Second attempt (broken .csproj): 30 minutes ❌
- Third attempt (fresh project): 15 minutes ✅
- Package installation + configuration: 20 minutes
- Testing + verification: 10 minutes
- Architecture questions + explanations: 40 minutes
- Git setup: 10 minutes

**Major time sinks:**
- Waiting for SQL Server to start (didn't know about 30 second delay)
- Troubleshooting namespace `_` issue
- Recreating project multiple times

---

## ✅ VERIFICATION - MODULE 0 COMPLETE

All these work:
- ✅ Docker Desktop running
- ✅ SQL Server container running and ready
- ✅ DBeaver connected to SQL Server
- ✅ Database "EmployeeManagementDB" created
- ✅ .NET SDK installed (version 10.0.103)
- ✅ ASP.NET MVC project created (no namespace issues)
- ✅ All NuGet packages installed
- ✅ Data/ and Services/ folders created
- ✅ appsettings.json has connection string
- ✅ Project builds without errors
- ✅ Application runs on http://localhost:5099
- ✅ Browser shows ASP.NET welcome page
- ✅ Git initialized and connected to GitHub

---

## 🚀 WHAT'S NEXT - MODULE 1

**Now ready to start:**
- Create Department.cs model
- Create Employee.cs model
- Create ApplicationDbContext.cs
- Configure Entity Framework
- Run first migration
- Create tables in SQL Server

**Estimated Time:** 2 hours

---

## 💡 TIPS FOR FUTURE SETUPS

### Before Starting
1. Make sure Docker Desktop is running
2. Have your password ready (write it down!)
3. Check .NET version: `dotnet --version`

### During Setup
1. Wait 30 seconds after starting SQL Server container
2. Check logs before trying to connect
3. Check .csproj file immediately after creating project
4. Never use spaces in project names
5. Save commands that work (for next time)

### If Issues
1. Check Docker logs first: `docker logs sqlserver2019`
2. Try `127.0.0.1` instead of `localhost`
3. Wait longer (SQL Server is slow to start)
4. When in doubt, recreate (faster than debugging)
5. Keep backups of working configs

---

## 📝 PERSONAL NOTES

**What went well:**
- Docker installation was smooth
- VS Code + .NET SDK worked great
- DBeaver is a good SQL client
- Fresh project worked first try (after we learned lessons)

**What was challenging:**
- Understanding SQL Server startup time
- DBeaver's "GO" error message was confusing
- Namespace `_` issue wasted time
- Multiple project recreations

**What we learned:**
- Wait for SQL Server logs to show "ready"
- Check .csproj immediately
- Don't use spaces in names
- Sometimes starting fresh is faster

**Would do differently next time:**
- Wait 30 seconds BEFORE trying to connect
- Check .csproj BEFORE building
- Use better project name validation
- Keep notes of what works

---

**Session End Time:** ~3 hours from start  
**Status:** Module 0 Complete ✅  
**Next Session:** Module 1 - Database & Entity Framework

**Files Backed Up:**
- Old broken project: `EmployeeManagementSystem_OLD/`
- Documentation: `Docs/` folder
- All settings documented in this file

**Ready to continue!** 🎉