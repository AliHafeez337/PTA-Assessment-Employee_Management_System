# Employee Management System (EMS)
## Technical Architecture Document

**📱 macOS Development | SQL Server via Docker (as per PDF specification)**

---

## 1. ARCHITECTURE OVERVIEW

### 1.1 What is MVC?

**MVC** stands for **Model-View-Controller**. It's a design pattern that separates your application into three parts:

```
┌─────────────────────────────────────────────┐
│                   USER                       │
│            (Uses web browser)                │
└────────────────┬────────────────────────────┘
                 │
                 ↓
┌────────────────────────────────────────────┐
│              VIEW (HTML Pages)              │
│        What the user sees and interacts    │
│              with (UI)                      │
└────────────────┬───────────────────────────┘
                 │
                 ↓
┌────────────────────────────────────────────┐
│           CONTROLLER (C# Code)              │
│     Receives requests, processes them,     │
│         talks to Model, returns View       │
└────────────────┬───────────────────────────┘
                 │
                 ↓
┌────────────────────────────────────────────┐
│            MODEL (Data Classes)             │
│        Represents data structure &         │
│          talks to Database                 │
└────────────────┬───────────────────────────┘
                 │
                 ↓
┌────────────────────────────────────────────┐
│          DATABASE (SQL Server)              │
│         Stores all the data                │
└────────────────────────────────────────────┘
```

**Simple Explanation:**
- **Model** = Data (what information we store)
- **View** = Display (what user sees)
- **Controller** = Logic (what happens when user clicks something)

---

## 2. PROJECT STRUCTURE

### 2.1 Folder Organization

```
EmployeeManagementSystem/
│
├── Controllers/                 ← Handles user requests
│   ├── HomeController.cs        ← Dashboard
│   ├── DepartmentController.cs  ← Department operations
│   └── EmployeeController.cs    ← Employee operations
│
├── Models/                      ← Data structures
│   ├── Department.cs            ← Department data model
│   ├── Employee.cs              ← Employee data model
│   └── UploadResult.cs          ← Upload result model
│
├── Views/                       ← HTML pages
│   ├── Home/
│   │   └── Index.cshtml         ← Dashboard page
│   ├── Department/
│   │   ├── Index.cshtml         ← Department list
│   │   ├── Create.cshtml        ← Add department form
│   │   └── Edit.cshtml          ← Edit department form
│   ├── Employee/
│   │   ├── Index.cshtml         ← Employee list
│   │   ├── _CreateEmployee.cshtml  ← Add popup
│   │   ├── _EditEmployee.cshtml    ← Edit popup
│   │   └── Upload.cshtml        ← Upload page
│   └── Shared/
│       ├── _Layout.cshtml       ← Master page template
│       └── _ValidationScripts.cshtml
│
├── Data/                        ← Database related
│   └── ApplicationDbContext.cs  ← EF Core context
│
├── Services/                    ← Business logic
│   └── FileUploadService.cs     ← File processing
│
├── wwwroot/                     ← Static files
│   ├── css/
│   │   └── site.css             ← Custom styles
│   ├── js/
│   │   └── site.js              ← Custom JavaScript
│   └── lib/                     ← jQuery, Bootstrap
│
├── appsettings.json             ← Configuration
└── Program.cs                   ← Application entry point
```

---

## 3. TECHNOLOGY STACK DETAILS

### 3.1 Backend Technologies

**1. ASP.NET Core MVC 6.0**
- **What it is:** Framework for building web applications
- **Why we use it:** Structured, organized, easy to maintain
- **Language:** C#

**2. Entity Framework Core**
- **What it is:** ORM (Object-Relational Mapping)
- **Why we use it:** Don't need to write SQL queries manually
- **How it works:** Converts C# code to SQL automatically

**Example:**
```csharp
// Instead of writing SQL:
// "SELECT * FROM Employees WHERE DepartmentId = 1"

// We write C# code:
var employees = dbContext.Employees
    .Where(e => e.DepartmentId == 1)
    .ToList();
```

**3. SQL Server**
- **What it is:** Database management system
- **On macOS:** Running via Docker container
- **Why we use it:** Required by project specification, reliable, powerful, industry standard
- **Version:** SQL Server 2019 (in Docker)

**Database Tool:**
- **DBeaver** - Universal database client for managing SQL Server

### 3.2 Frontend Technologies

**1. HTML/CSS**
- **What it is:** Structure and styling of web pages
- **Why we use it:** Standard for web development

**2. Bootstrap 5**
- **What it is:** CSS framework with pre-built components
- **Why we use it:** Makes pages look professional quickly
- **Components we'll use:** 
  - Grid system
  - Forms
  - Tables
  - Modals
  - Buttons
  - Alerts

**3. jQuery 3.6**
- **What it is:** JavaScript library
- **Why we use it:** Simplifies JavaScript code
- **What we'll use it for:**
  - AJAX calls
  - Form validation
  - DOM manipulation
  - Event handling

**4. JavaScript**
- **What it is:** Programming language for browsers
- **Why we use it:** Makes pages interactive

---

## 4. DATA FLOW

### 4.1 How a Request Works

**Example: User wants to add a new employee**

```
STEP 1: User Action
User clicks "Add Employee" button on webpage
        ↓

STEP 2: Browser Request
Browser sends request to server: GET /Employee/Create
        ↓

STEP 3: Controller Receives
EmployeeController.Create() method is called
        ↓

STEP 4: Controller Returns View
Controller returns Create.cshtml view with empty form
        ↓

STEP 5: User Fills Form
User enters employee details and clicks "Save"
        ↓

STEP 6: Form Submission
Browser sends POST request to: POST /Employee/Create
Data: Name, Email, Salary, DepartmentId, JoiningDate
        ↓

STEP 7: Controller Validates
EmployeeController.Create(Employee employee) validates data
        ↓

STEP 8: Save to Database
If valid: Controller uses Entity Framework to save
dbContext.Employees.Add(employee)
dbContext.SaveChanges()
        ↓

STEP 9: Response
Controller redirects to Employee List page
Browser shows updated list with new employee
```

### 4.2 AJAX Request Flow

**Example: Get employee count for a department**

```
STEP 1: User Selects Department
User selects "IT" from department dropdown
        ↓

STEP 2: JavaScript Event
onChange event is triggered
        ↓

STEP 3: AJAX Call
$.ajax({
    url: '/Employee/GetDepartmentEmployeeCount',
    data: { departmentId: 1 },
    success: function(count) { ... }
})
        ↓

STEP 4: Controller Method
EmployeeController.GetDepartmentEmployeeCount(int id)
Queries database for count
        ↓

STEP 5: Return JSON
Controller returns: { count: 25 }
        ↓

STEP 6: Update UI
JavaScript displays: "This department has 25 employees"
(Page does NOT reload)
```

---

## 5. DATABASE DESIGN

### 5.1 Entity Relationship Diagram

```
┌─────────────────────────────┐
│       DEPARTMENTS           │
├─────────────────────────────┤
│ DepartmentId (PK)           │
│ DepartmentName              │
│ DepartmentCode              │
│ ActiveInactive              │
│ CreatedDate                 │
└─────────────┬───────────────┘
              │
              │ 1
              │
              │ has many
              │
              │ *
              │
┌─────────────┴───────────────┐
│        EMPLOYEES            │
├─────────────────────────────┤
│ EmployeeId (PK)             │
│ Name                        │
│ Email (Unique)              │
│ Salary                      │
│ DepartmentId (FK)           │◄── Links to Departments
│ JoiningDate                 │
└─────────────────────────────┘
```

**Relationship Explanation:**
- One Department can have Many Employees
- Each Employee belongs to One Department
- If we delete a Department, we should handle Employees (cascade or prevent)

### 5.2 Table Details

**Departments Table:**
```sql
CREATE TABLE Departments (
    DepartmentId INT PRIMARY KEY IDENTITY(1,1),  -- Auto-increment
    DepartmentName NVARCHAR(100) NOT NULL,
    DepartmentCode NVARCHAR(20) NOT NULL,
    ActiveInactive BIT NOT NULL DEFAULT 1,        -- 1=Active, 0=Inactive
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
```

**Employees Table:**
```sql
CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),    -- Auto-increment
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,          -- Must be unique
    Salary DECIMAL(18,2) NOT NULL,                -- 18 digits, 2 decimal places
    DepartmentId INT NOT NULL,
    JoiningDate DATE NOT NULL,
    CONSTRAINT FK_Employee_Department 
        FOREIGN KEY (DepartmentId) 
        REFERENCES Departments(DepartmentId)
);
```

---

## 6. KEY COMPONENTS EXPLAINED

### 6.1 Models (C# Classes)

**What they are:** C# classes that represent database tables

**Department.cs:**
```csharp
public class Department
{
    public int DepartmentId { get; set; }          // Primary key
    public string DepartmentName { get; set; }     // Department name
    public string DepartmentCode { get; set; }     // Short code
    public bool ActiveInactive { get; set; }       // Active status
    public DateTime CreatedDate { get; set; }      // Creation date
    
    // Navigation property - list of employees in this department
    public List<Employee> Employees { get; set; }
}
```

**Employee.cs:**
```csharp
public class Employee
{
    public int EmployeeId { get; set; }           // Primary key
    public string Name { get; set; }              // Employee name
    public string Email { get; set; }             // Unique email
    public decimal Salary { get; set; }           // Salary
    public int DepartmentId { get; set; }         // Foreign key
    public DateTime JoiningDate { get; set; }     // Joining date
    
    // Navigation property - reference to department
    public Department Department { get; set; }
}
```

### 6.2 Controllers (Request Handlers)

**What they do:** Handle HTTP requests and return responses

**Basic structure:**
```csharp
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;
    
    // Constructor - receives database context
    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET: Shows employee list
    public IActionResult Index()
    {
        var employees = _context.Employees.ToList();
        return View(employees);
    }
    
    // POST: Saves new employee
    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
```

### 6.3 Views (HTML Templates)

**What they are:** HTML files with C# code embedded

**Example - Employee List:**
```html
@model List<Employee>

<h2>Employee List</h2>

<table class="table">
    <thead>
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Salary</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach(var employee in Model)
        {
            <tr>
                <td>@employee.Name</td>
                <td>@employee.Email</td>
                <td>@employee.Salary</td>
                <td>
                    <a href="/Employee/Edit/@employee.EmployeeId">Edit</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

### 6.4 Entity Framework DbContext

**What it is:** Bridge between C# code and database

```csharp
public class ApplicationDbContext : DbContext
{
    // Define tables
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    
    // Constructor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    // Configure relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One Department has many Employees
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId);
    }
}
```

---

## 7. LIBRARIES & PACKAGES

### 7.1 NuGet Packages (C# Libraries)

**What is NuGet?** Package manager for .NET (like npm for Node.js)

**Packages we'll use:**

1. **Microsoft.EntityFrameworkCore.SqlServer**
   - Version: 6.0 or later
   - Purpose: Connect to SQL Server

2. **Microsoft.EntityFrameworkCore.Tools**
   - Version: 6.0 or later
   - Purpose: Create database migrations

3. **EPPlus** (for Excel reading)
   - Version: 6.0 or later
   - Purpose: Read Excel files (.xlsx)

4. **CsvHelper** (optional, for CSV reading)
   - Version: 30.0 or later
   - Purpose: Read CSV files easily

### 7.2 Frontend Libraries

**Already included in ASP.NET MVC:**

1. **Bootstrap 5**
   - CSS Framework
   - Location: wwwroot/lib/bootstrap/

2. **jQuery 3.6**
   - JavaScript Library
   - Location: wwwroot/lib/jquery/

3. **jQuery Validation**
   - Form validation plugin
   - Location: wwwroot/lib/jquery-validation/

---

## 8. CONFIGURATION FILES

### 8.1 appsettings.json

**What it is:** Configuration file for the application

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmployeeManagementDB;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### 8.2 Program.cs

**What it is:** Application entry point

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure middleware
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

---

## 9. DESIGN PATTERNS

### 9.1 Repository Pattern (Optional)

**Not required for this project, but good to know:**

Instead of accessing DbContext directly in controllers, we can create a repository layer.

**Benefits:**
- Cleaner code
- Easier to test
- Better separation of concerns

**We'll use direct DbContext access for simplicity.**

### 9.2 Dependency Injection

**What it is:** Providing dependencies to classes automatically

**Example:**
```csharp
// Instead of creating DbContext manually:
var context = new ApplicationDbContext();

// We let ASP.NET inject it:
public EmployeeController(ApplicationDbContext context)
{
    _context = context;  // Automatically provided
}
```

---

## 10. SECURITY CONSIDERATIONS

### 10.1 What We'll Implement

✅ **Input Validation**
- Prevent SQL injection (Entity Framework handles this)
- Validate email format
- Sanitize user input

✅ **Error Handling**
- Don't show technical errors to users
- Log errors internally
- Show user-friendly messages

### 10.2 What's Out of Scope (Not Required)

❌ Authentication (login system)
❌ Authorization (user roles)
❌ HTTPS enforcement
❌ Password encryption
❌ CSRF protection (should be enabled by default)

---

## 11. PERFORMANCE CONSIDERATIONS

### 11.1 Database Queries

**Good practice:**
```csharp
// Load related data efficiently
var employees = _context.Employees
    .Include(e => e.Department)  // Eager loading
    .ToList();
```

**Avoid:**
```csharp
// This causes N+1 query problem
foreach(var employee in employees)
{
    var dept = _context.Departments.Find(employee.DepartmentId);
}
```

### 11.2 AJAX vs Full Page Reload

**AJAX (Better):**
- Only sends/receives necessary data
- Page doesn't reload
- Faster user experience

**Full Reload (Simpler):**
- Entire page reloads
- Easier to implement
- Acceptable for this project

---

## 12. TESTING STRATEGY

### 12.1 Manual Testing

**What we'll do:**
1. Test each feature after implementation
2. Test validation rules
3. Test error scenarios
4. Test with different browsers
5. Test with large data sets

### 12.2 Test Cases (We'll create detailed list later)

**Example:**
- ✅ Add employee with valid data → Should succeed
- ✅ Add employee with duplicate email → Should show error
- ✅ Upload CSV with 1000 records → Should process correctly
- ✅ Delete department with employees → Should handle gracefully

---

## 13. DEPLOYMENT (FINAL STEP)

### 13.1 What We'll Deliver

1. **Source Code** - Complete Visual Studio solution
2. **Database Script** - SQL script to create tables
3. **Sample Files** - Example CSV for upload
4. **Documentation** - README with setup instructions

### 13.2 Running the Application

```
1. Install prerequisites
2. Open solution in Visual Studio
3. Update connection string in appsettings.json
4. Run migrations to create database
5. Press F5 to run application
6. Browser opens automatically
```

---

**Document Version:** 1.0  
**Last Updated:** February 17, 2026  
**Status:** Draft - Awaiting Approval