# Employee Management System (EMS)
## Setup Instructions & Prerequisites

**📱 This version is customized for macOS with DBeaver**

---

## 1. SYSTEM REQUIREMENTS

### Minimum Requirements:
- **Operating System:** macOS (your system)
- **RAM:** 8 GB (16 GB recommended)
- **Disk Space:** 10 GB free space
- **Processor:** Intel Core i5 or Apple Silicon (M1/M2/M3)
- **Internet Connection:** Required for downloads and NuGet packages

### Your Current Setup:
- ✅ **MacBook** - Confirmed
- ✅ **DBeaver** - Already installed
- ❓ **SQL Server** - Need to setup (see options below)

---

## 2. SOFTWARE TO INSTALL (macOS Version)

We need to install 2 main tools + choose a database option:

### 2.1 Visual Studio for Mac / Visual Studio Code

**Two Options for macOS:**

#### Option A: Visual Studio for Mac (Recommended for Beginners)
**What it is:** Full-featured IDE specifically designed for Mac

**Download Link:** https://visualstudio.microsoft.com/vs/mac/

**Installation Steps:**
1. Download Visual Studio for Mac
2. Open the .dmg file
3. Drag Visual Studio to Applications folder
4. Launch Visual Studio
5. During first launch, select:
   - ✅ **.NET** (Core)
   - ✅ **ASP.NET and web development**
6. Complete installation (takes 20-30 minutes)

#### Option B: Visual Studio Code + .NET SDK (Lighter Alternative)
**What it is:** Lightweight code editor

**Installation Steps:**
1. **Install VS Code:**
   - Download from: https://code.visualstudio.com/
   - Open .dmg and drag to Applications
   
2. **Install .NET SDK:**
   ```bash
   # Using Homebrew
   brew install --cask dotnet-sdk
   ```
   
3. **Install VS Code Extensions:**
   - Open VS Code
   - Install "C#" extension by Microsoft
   - Install "C# Dev Kit" extension

**For this tutorial, we'll assume Visual Studio for Mac, but I'll note differences if using VS Code.**

---

### 2.2 SQL Server Setup for macOS

Since you have **DBeaver** already installed, you just need to setup SQL Server.

**The project specification requires SQL Server.** On macOS, we run SQL Server via Docker:

#### SQL Server via Docker 🐳 (Required for macOS)

**What it is:** SQL Server running in a Docker container

**Why this option:**
- ✅ Meets project requirement (actual SQL Server)
- ✅ Works perfectly with DBeaver
- ✅ Industry-standard approach for macOS

**Installation Steps:**

**Step 1: Install Docker Desktop**
- Download from: https://www.docker.com/products/docker-desktop/
- Open .dmg file and install
- Launch Docker Desktop
- Accept terms and complete setup

**Step 2: Run SQL Server Container**
Open Terminal and run:
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sqlserver2019 \
   -d mcr.microsoft.com/mssql/server:2019-latest
```

**Important Notes:**
- Use `MSSQL_SA_PASSWORD` (not `SA_PASSWORD`)
- You'll see a platform warning on Apple Silicon Macs - **this is normal and expected**:
  ```
  WARNING: The requested image's platform (linux/amd64) does not match 
  the detected host platform (linux/arm64/v8)
  ```
  This means SQL Server runs via emulation (Rosetta 2) - works perfectly for development!

**What this does:**
- Downloads SQL Server 2019 image (first time takes a few minutes)
- Creates a container named "sqlserver2019"
- Sets password to "YourStrong@Passw0rd" (you can change this)
- Exposes port 1433 (SQL Server default port)

**Step 3: Verify Container is Running**
```bash
docker ps
```

**Expected output:**
```
CONTAINER ID   IMAGE                                        COMMAND                  CREATED          STATUS         PORTS                    NAMES
e61263256862   mcr.microsoft.com/mssql/server:2019-latest   "/opt/mssql/bin/perm…"   2 minutes ago    Up 10 seconds  0.0.0.0:1433->1433/tcp   sqlserver2019
```

You should see "sqlserver2019" in the list with port mapping "0.0.0.0:1433->1433/tcp"

**Step 3b: Wait for SQL Server to Start**

**IMPORTANT:** The container showing "Up" doesn't mean SQL Server is ready. Wait 20-30 seconds, then check logs:

```bash
docker logs sqlserver2019
```

**Look for this line at the end:**
```
SQL Server is now ready for client connections.
```

If you don't see it, wait 10 more seconds and check again.

**Step 4: Connect with DBeaver**
1. Open DBeaver
2. Click "New Database Connection"
3. Select "SQL Server" (if not visible, install the driver)
4. Enter connection details:
   - **Server Host:** `localhost`
   - **Port:** `1433`
   - **Database:** `master` (initially)
   - **Authentication:** SQL Server Authentication
   - **Username:** `sa`
   - **Password:** `YourStrong@Passw0rd`
5. Click "Test Connection"
6. If successful, click "Finish"

**Managing the Container:**
```bash
# Start container (if stopped)
docker start sqlserver2019

# Stop container
docker stop sqlserver2019

# View logs
docker logs sqlserver2019
```

---

## 3. WHICH SETUP YOU NEED

**Summary:**
- ✅ Visual Studio for Mac (or VS Code)
- ✅ Docker Desktop
- ✅ SQL Server 2019 running in Docker container
- ✅ DBeaver (already have it!)

**We are using SQL Server exactly as specified in the project PDF.** No alternatives, no deviations.

---

### 2.3 DBeaver (You Already Have This! ✅)

**What it is:** Universal database tool that works with many databases

**You already have this installed, so we just need to:**
1. Make sure it's updated to latest version
2. Connect it to SQL Server (Docker container)

---

---

## 4. ENVIRONMENT SETUP CHECKLIST

Before we start coding, verify everything is installed:

```
✅ Visual Studio for Mac installed (or VS Code + .NET SDK)
✅ Can create new project in Visual Studio
✅ Docker Desktop installed and running
✅ SQL Server container running in Docker
✅ Can connect to SQL Server using DBeaver
✅ DBeaver is up to date
✅ Internet connection working
✅ Have at least 10GB free disk space
```

**Your Specific Setup:**
- ✅ MacBook
- ✅ DBeaver already installed
- ⏳ Install Docker Desktop
- ⏳ Setup SQL Server in Docker
- ⏳ Install Visual Studio for Mac

---

## 5. CREATING THE PROJECT (macOS)

### Using Visual Studio for Mac:

#### Step 1: Open Visual Studio for Mac
- Launch Visual Studio from Applications folder
- Click "New" or "New Project"

#### Step 2: Choose Project Template
1. In the left sidebar, select: **Web and Console → App**
2. Select: **ASP.NET Core → Web Application (Model-View-Controller)**
3. Click "Continue"

#### Step 3: Configure Project
1. **Target Framework:** Select **.NET 6.0** or **.NET 7.0** or **.NET 8.0** (latest available)
2. Click "Continue"

#### Step 4: Project Details
1. **Project Name:** `EmployeeManagementSystem`
2. **Solution Name:** `EmployeeManagementSystem`
3. **Location:** Choose where to save (e.g., `~/Projects/EmployeeManagementSystem`)
4. **Create a git repository:** (optional) ✅ Check if you want version control
5. Click "Create"

#### Step 5: Wait for Project Creation
- Visual Studio will create the project structure
- It will download required NuGet packages
- Should take 30-60 seconds

### Using VS Code (Alternative):

If using VS Code instead:

```bash
# Open Terminal
cd ~/Projects

# Create new project
dotnet new mvc -n EmployeeManagementSystem

# Navigate into project
cd EmployeeManagementSystem

# Open in VS Code
code .
```

#### Step 6: Verify Project Created
You should see in Solution Explorer:
```
EmployeeManagementSystem/
├── Dependencies
├── Properties
├── wwwroot/
├── Controllers/
│   └── HomeController.cs
├── Models/
├── Views/
│   ├── Home/
│   └── Shared/
├── appsettings.json
└── Program.cs
```

---

## 6. INITIAL PROJECT TEST (macOS)

Let's make sure the project runs:

#### Step 1: Run the Project

**In Visual Studio for Mac:**
- Click the "Play" button (▶) at top, OR
- Press **⌘ + Return** (Command + Return)

**In VS Code:**
```bash
dotnet run
```

#### Step 2: Access the Application
- Terminal will show output like:
```
Now listening on: https://localhost:7xxx
```
- Copy the URL
- Open your browser (Safari, Chrome, etc.)
- Paste the URL

**Note:** You may see a certificate warning on first run - this is normal for HTTPS development. Click "Advanced" and "Proceed" (different browsers have different wording).

#### Step 3: Expected Result
- ✅ Browser shows "Welcome" or "Home" page
- ✅ No error messages
- ✅ Can navigate between "Home" and "Privacy" pages

#### Step 4: Stop the Application
**In Visual Studio for Mac:**
- Click the "Stop" button (⏹) at top, OR
- Close the browser and press **Shift + ⌘ + Return**

**In VS Code:**
- Press **Control + C** in the terminal

**If you see the welcome page, congratulations! Project setup is complete! 🎉**

---

## 7. CREATE DATABASE (Using DBeaver)

Now let's create the database using DBeaver and SQL Server:

#### Step 1: Ensure Docker Container is Running
```bash
# Check if container is running
docker ps

# If not running, start it
docker start sqlserver2019
```

#### Step 2: Open DBeaver
- Launch DBeaver
- You should see your SQL Server connection in the left panel
- If not, create connection as described in section 2.2

#### Step 3: Create Database
1. **Connect to SQL Server:**
   - Double-click your connection in DBeaver
   - It should connect to the `master` database

2. **Open SQL Editor:**
   - Right-click on your connection
   - Select "SQL Editor" → "New SQL Script"

3. **Create Database:**
   - Type this SQL command:
   ```sql
   CREATE DATABASE EmployeeManagementDB;
   GO
   ```

4. **Execute:**
   - Press **⌘ + Return** (or click Execute button)
   - You should see "Query executed successfully"

5. **Verify Database Created:**
   - Right-click on connection → "Refresh"
   - Expand "Databases" folder
   - You should see "EmployeeManagementDB"

#### Step 4: Test Database Connection
1. In DBeaver, double-click "EmployeeManagementDB"
2. It should connect without errors
3. You should see default database objects (tables, views, etc.)

**Success! Your database is ready! ✅**

---

## 8. CONFIGURE DATABASE CONNECTION (macOS)

#### Step 1: Open appsettings.json
- In Solution Explorer/File Explorer, find `appsettings.json`
- Double-click to open

#### Step 2: Add Connection String for SQL Server

Replace the entire content with:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EmployeeManagementDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
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

**Important Notes:**
- `Server=localhost,1433` - Note the **comma** (not colon on Mac)
- `User Id=sa` - SQL Server admin user
- `Password=YourStrong@Passw0rd` - **Replace with your actual password from section 2.2**
- `TrustServerCertificate=True` - Required for Docker SQL Server

#### Step 3: Save File
- Press **⌘ + S** (Command + S) to save

**Security Note:** 
Never commit passwords to version control. For production, use environment variables or Azure Key Vault.

---

## 9. INSTALL REQUIRED NUGET PACKAGES (macOS)

### Using Visual Studio for Mac:

#### Step 1: Open Package Manager Console
- **Project → Manage NuGet Packages**

OR use Terminal:

#### Using Terminal (Recommended for macOS):

Navigate to your project folder:
```bash
cd ~/Projects/EmployeeManagementSystem
```

### Install Required Packages for SQL Server:

```bash
# Entity Framework Core for SQL Server
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0

# EF Core Tools (for migrations)
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

# EF Core Design (for migrations)
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# For Excel file processing (we'll need this later)
dotnet add package EPPlus --version 7.0.0
```

#### Step 2: Verify Installation

After running the commands, check your project file:

```bash
cat EmployeeManagementSystem.csproj
```

You should see package references like:
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
  <PackageReference Include="EPPlus" Version="7.0.0" />
</ItemGroup>
```

**Success! Packages are installed! ✅**

---

## 10. PROJECT FOLDER STRUCTURE

Let's create the folders we'll need:

#### Step 1: Create Data Folder
1. Right-click on project name (EmployeeManagementSystem)
2. Add → New Folder
3. Name it: **`Data`**

#### Step 2: Create Services Folder
1. Right-click on project name
2. Add → New Folder
3. Name it: **`Services`**

#### Current Structure:
```
EmployeeManagementSystem/
├── Controllers/
├── Data/           ← We just created this
├── Models/
├── Services/       ← We just created this
├── Views/
└── wwwroot/
```

---

## 10. FINAL VERIFICATION CHECKLIST

Before we start Module 1, verify:

```
✅ Visual Studio 2022 is installed and working
✅ SQL Server Express is installed
✅ SQL Server Management Studio is installed
✅ Can connect to SQL Server in SSMS
✅ Created ASP.NET Core MVC project
✅ Project runs successfully (shows welcome page)
✅ Database "EmployeeManagementDB" created
✅ Connection string configured in appsettings.json
✅ NuGet packages installed
✅ Data and Services folders created
```

---

## 12. TROUBLESHOOTING (macOS)

### Problem: Visual Studio for Mac won't install
**Solution:**
- Check if you have enough disk space (need 10GB)
- Ensure macOS version is compatible (macOS 10.15 or later)
- Try downloading again
- Restart Mac and try again

### Problem: Can't connect to SQL Server (Docker)
**Solution:**
- Verify Docker Desktop is running:
  ```bash
  docker ps
  ```
- Check if container is running:
  ```bash
  docker ps | grep sqlserver
  ```
- If container stopped, start it:
  ```bash
  docker start sqlserver2019
  ```
- Check container logs:
  ```bash
  docker logs sqlserver2019
  ```
- Try recreating the container (stop and remove first):
  ```bash
  docker stop sqlserver2019
  docker rm sqlserver2019
  # Then run the docker run command again
  ```

### Problem: DBeaver can't connect to SQL Server
**Solution:**
- Ensure Docker container is running (`docker ps`)
- Verify port 1433 is not blocked
- Check password is correct
- Try `127.0.0.1` instead of `localhost`
- Check container logs: `docker logs sqlserver2019`

### Problem: NuGet package installation fails
**Solution:**
- Check internet connection
- Clear NuGet cache:
  ```bash
  dotnet nuget locals all --clear
  ```
- Try again:
  ```bash
  dotnet restore
  ```
- If using Visual Studio for Mac:
  - Project → Restore NuGet Packages

### Problem: Project won't run (error on ⌘+Return)
**Solution:**
- Check terminal for error messages
- Clean and rebuild:
  ```bash
  dotnet clean
  dotnet build
  ```
- Check if port is already in use
- Try running on different port:
  ```bash
  dotnet run --urls="https://localhost:5001;http://localhost:5000"
  ```

### Problem: Permission denied errors
**Solution:**
- Some commands may need elevated permissions:
  ```bash
  sudo command-here
  ```
- For Docker issues, ensure Docker Desktop has necessary permissions in:
  - System Preferences → Security & Privacy

### Problem: Certificate/HTTPS errors
**Solution:**
- Trust the development certificate:
  ```bash
  dotnet dev-certs https --trust
  ```
- Restart browser
- Clear browser cache
- Try a different browser

### Problem: "Cannot find dotnet" error
**Solution:**
- Verify .NET SDK is installed:
  ```bash
  dotnet --version
  ```
- If not installed:
  ```bash
  brew install --cask dotnet-sdk
  ```
- Restart terminal after installation
- Add to PATH if needed:
  ```bash
  export PATH="$PATH:/usr/local/share/dotnet"
  ```

---

## 13. KEYBOARD SHORTCUTS (macOS - Helpful)

While working in Visual Studio for Mac:

```
⌘ + Return           - Run the application
Shift + ⌘ + Return   - Stop the application
⌘ + S                - Save file
⌘ + B                - Build project
⌃ + F5               - Run without debugging
F12 or ⌘ + D         - Go to definition
⌘ + .                - Show quick actions
⌘ + K, ⌘ + D         - Format document
⌘ + /                - Comment/uncomment line
⌘ + Shift + F        - Find in files
⌘ + T                - Go to file
⌥ + ↑/↓              - Move line up/down
```

### Terminal Shortcuts (When using VS Code):

```
⌘ + `                - Toggle terminal
⌃ + C                - Stop running process
⌘ + K                - Clear terminal
Tab                  - Auto-complete
```

---

## 14. RECOMMENDED VISUAL STUDIO EXTENSIONS (OPTIONAL)

These make development easier:

1. **Web Essentials** - Better HTML/CSS/JS editing
2. **Productivity Power Tools** - General productivity enhancements
3. **CodeMaid** - Code cleanup and formatting
4. **Git Extensions** - Better Git integration

To install:
- Extensions → Manage Extensions
- Search for extension name
- Click "Download"
- Restart Visual Studio

---

## 15. NEXT STEPS

✅ **If everything above is working, you're ready to start Module 1!**

We'll begin by:
1. Creating Model classes (Department.cs and Employee.cs)
2. Creating ApplicationDbContext
3. Running our first migration
4. Creating database tables in DBeaver

**Using SQL Server as specified in the project PDF - no deviations!**

---

## 16. GETTING HELP

If you get stuck:

1. **Check Error Message**
   - Read the full error in Terminal/Output
   - Google the error message
   - Stack Overflow usually has answers

2. **Visual Studio for Mac Tools**
   - Errors pad (View → Pads → Errors)
   - Build output (View → Pads → Build Output)
   - Application output (View → Pads → Application Output)

3. **Terminal Commands**
   ```bash
   # Check .NET version
   dotnet --version
   
   # Check project info
   dotnet --info
   
   # Restore packages
   dotnet restore
   
   # Build project
   dotnet build
   ```

4. **Docker Commands** (if using SQL Server)
   ```bash
   # Check container status
   docker ps -a
   
   # View logs
   docker logs sqlserver2019
   
   # Restart container
   docker restart sqlserver2019
   ```

5. **Ask Me**
   - Share the error message
   - Share what you were trying to do
   - Share any relevant code
   - Mention if you're using SQL Server or PostgreSQL

---

**Document Version:** 2.0 (macOS Edition)  
**Last Updated:** February 17, 2026  
**Status:** Ready for Use - Customized for macOS with DBeaver

**Ready to proceed? Let me know when setup is complete!** 🚀

---

## Quick Reference Card

Keep this handy:

### Your Setup:
- **OS:** macOS
- **IDE:** Visual Studio for Mac (or VS Code)
- **Database Tool:** DBeaver ✅
- **Database:** [ ] SQL Server (Docker) or [ ] PostgreSQL
- **Project Location:** _________________
- **Database Password:** _________________ (keep secure!)

### Common Commands:
```bash
# Run project
dotnet run

# Build project
dotnet build

# Add package
dotnet add package PackageName

# Migrations (we'll use these in Module 1)
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Docker (if using SQL Server):
```bash
# Start SQL Server
docker start sqlserver2019

# Stop SQL Server
docker stop sqlserver2019

# Check status
docker ps
```