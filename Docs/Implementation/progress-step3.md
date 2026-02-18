# Employee Management System — Actual Setup Steps
## Everything we did in this project, in order

**OS:** macOS (Apple Silicon — ARM64)  
**Date:** February 17-18, 2026

---

## STEP 1: Install Docker Desktop

1. Download Docker Desktop from https://www.docker.com/products/docker-desktop/
2. Open the `.dmg` file and install
3. Launch Docker Desktop and accept terms

---

## STEP 2: Run SQL Server in Docker

Open Terminal and run:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sqlserver2019 \
   -d mcr.microsoft.com/mssql/server:2019-latest
```

**Note:** On Apple Silicon you'll see a platform warning — this is normal, SQL Server runs via Rosetta emulation and works perfectly.

Verify it's running:
```bash
docker ps
```

Wait 20-30 seconds then check logs:
```bash
docker logs sqlserver2019
```
Look for: `SQL Server is now ready for client connections.`

**Managing the container later:**
```bash
docker start sqlserver2019    # start
docker stop sqlserver2019     # stop
docker ps                     # check status
```

---

## STEP 3: Connect DBeaver to SQL Server

1. Open DBeaver → New Database Connection → SQL Server
2. Enter:
   - **Host:** `localhost`
   - **Port:** `1433`
   - **Database:** `master`
   - **Authentication:** SQL Server Authentication
   - **Username:** `sa`
   - **Password:** `YourStrong@Passw0rd`
3. Test Connection → Finish

---

## STEP 4: Create the Database

In DBeaver:
1. Right-click your connection → SQL Editor → New SQL Script
2. Run this (no `GO` — that's SSMS only):
```sql
CREATE DATABASE EmployeeManagementDB;
```
3. Right-click connection → Refresh → you should see `EmployeeManagementDB`

---

## STEP 5: Install .NET SDK

```bash
brew install --cask dotnet-sdk
```

Verify:
```bash
dotnet --version
# Should show 10.x.x
```

---

## STEP 6: Create the ASP.NET Core MVC Project

```bash
cd /Volumes/Office/Assessments/PTA

dotnet new mvc -n EmployeeManagementSystem

cd EmployeeManagementSystem
```

---

## STEP 7: Configure the Connection String

Open `appsettings.json` and replace with:

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

**Important:** Use a comma between `localhost` and `1433` (not a colon).

---

## STEP 8: Create Project Folders

```bash
mkdir Data
mkdir Services
```

Or create them manually in VS Code explorer.

---

## STEP 9: Install EF Core NuGet Packages

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
```

Install the EF CLI tool globally:
```bash
dotnet tool install --global dotnet-ef
```

---

## STEP 10: Test Run the Project

```bash
dotnet run
```

Open browser at `http://localhost:5099` — should show the default MVC welcome page.

Stop with `Ctrl + C`.

---

## STEP 11: Create `Models/Department.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department code is required")]
        [StringLength(20, ErrorMessage = "Code cannot exceed 20 characters")]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool ActiveInactive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
```

---

## STEP 12: Create `Models/Employee.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Please select a department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Joining date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }
    }
}
```

---

## STEP 13: Create `Data/ApplicationDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);
                entity.Property(d => d.DepartmentName).IsRequired().HasMaxLength(100);
                entity.Property(d => d.DepartmentCode).IsRequired().HasMaxLength(20);
                entity.HasIndex(d => d.DepartmentName).IsUnique();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
```

---

## STEP 14: Update `Program.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
```

---

## STEP 15: Run EF Migration (Create Tables)

```bash
dotnet ef migrations add InitialCreate

dotnet ef database update
```

Then verify in DBeaver — you should see 3 tables under `EmployeeManagementDB`:
- `Departments`
- `Employees`
- `__EFMigrationsHistory`

---

## STEP 16: Create `Controllers/DepartmentController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /Department
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
            return View(departments);
        }

        // GET /Department/Create
        public IActionResult Create()
        {
            return View(new Department());
        }

        // POST /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            bool nameExists = await _context.Departments
                .AnyAsync(d => d.DepartmentName == department.DepartmentName);

            if (nameExists)
                ModelState.AddModelError("DepartmentName", "A department with this name already exists.");

            if (ModelState.IsValid)
            {
                department.CreatedDate = DateTime.Now;
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // GET /Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();
            return View(department);
        }

        // POST /Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.DepartmentId) return NotFound();

            bool nameExists = await _context.Departments
                .AnyAsync(d => d.DepartmentName == department.DepartmentName
                            && d.DepartmentId != id);

            if (nameExists)
                ModelState.AddModelError("DepartmentName", "A department with this name already exists.");

            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // POST /Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## STEP 17: Create `Views/Department/Index.cshtml`

```html
@model List<EmployeeManagementSystem.Models.Department>
@{ ViewData["Title"] = "Departments"; }

<div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2>Departments</h2>
        <a asp-action="Create" class="btn btn-primary">+ Add Department</a>
    </div>

    @if (TempData["SuccessMessage"] != null)
    {
        <div class="alert alert-success alert-dismissible fade show" role="alert">
            @TempData["SuccessMessage"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }

    @if (Model.Count == 0)
    {
        <div class="alert alert-info">No departments found. Click "Add Department" to create one.</div>
    }
    else
    {
        <table class="table table-striped table-hover">
            <thead class="table-dark">
                <tr>
                    <th>#</th>
                    <th>Department Name</th>
                    <th>Code</th>
                    <th>Status</th>
                    <th>Created Date</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var dept in Model)
                {
                    <tr>
                        <td>@dept.DepartmentId</td>
                        <td>@dept.DepartmentName</td>
                        <td>@dept.DepartmentCode</td>
                        <td>
                            @if (dept.ActiveInactive)
                            {
                                <span class="badge bg-success">Active</span>
                            }
                            else
                            {
                                <span class="badge bg-danger">Inactive</span>
                            }
                        </td>
                        <td>@dept.CreatedDate.ToString("MMM dd, yyyy")</td>
                        <td>
                            <a asp-action="Edit" asp-route-id="@dept.DepartmentId"
                               class="btn btn-sm btn-warning">Edit</a>
                            <form asp-action="Delete" asp-route-id="@dept.DepartmentId"
                                  method="post" style="display:inline"
                                  onsubmit="return confirm('Delete @dept.DepartmentName?')">
                                @Html.AntiForgeryToken()
                                <button type="submit" class="btn btn-sm btn-danger">Delete</button>
                            </form>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
        <p class="text-muted">Total: @Model.Count department(s)</p>
    }
</div>
```

---

## STEP 18: Create `Views/Department/Create.cshtml`

```html
@model EmployeeManagementSystem.Models.Department
@{ ViewData["Title"] = "Add Department"; }

<div class="container mt-4">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <h2>Add Department</h2>
            <hr />
            <form asp-action="Create" method="post">
                @Html.AntiForgeryToken()
                <div class="mb-3">
                    <label asp-for="DepartmentName" class="form-label"></label>
                    <input asp-for="DepartmentName" class="form-control" placeholder="e.g. Information Technology" />
                    <span asp-validation-for="DepartmentName" class="text-danger"></span>
                </div>
                <div class="mb-3">
                    <label asp-for="DepartmentCode" class="form-label"></label>
                    <input asp-for="DepartmentCode" class="form-control" placeholder="e.g. IT" />
                    <span asp-validation-for="DepartmentCode" class="text-danger"></span>
                </div>
                <div class="mb-3 form-check">
                    <input asp-for="ActiveInactive" class="form-check-input" />
                    <label asp-for="ActiveInactive" class="form-check-label">Active</label>
                </div>
                <div class="d-flex gap-2">
                    <button type="submit" class="btn btn-primary">Save Department</button>
                    <a asp-action="Index" class="btn btn-secondary">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    @{ await Html.RenderPartialAsync("_ValidationScriptsPartial"); }
}
```

---

## STEP 19: Create `Views/Department/Edit.cshtml`

```html
@model EmployeeManagementSystem.Models.Department
@{ ViewData["Title"] = "Edit Department"; }

<div class="container mt-4">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <h2>Edit Department</h2>
            <hr />
            <form asp-action="Edit" method="post">
                @Html.AntiForgeryToken()
                <input type="hidden" asp-for="DepartmentId" />
                <input type="hidden" asp-for="CreatedDate" />
                <div class="mb-3">
                    <label asp-for="DepartmentName" class="form-label"></label>
                    <input asp-for="DepartmentName" class="form-control" />
                    <span asp-validation-for="DepartmentName" class="text-danger"></span>
                </div>
                <div class="mb-3">
                    <label asp-for="DepartmentCode" class="form-label"></label>
                    <input asp-for="DepartmentCode" class="form-control" />
                    <span asp-validation-for="DepartmentCode" class="text-danger"></span>
                </div>
                <div class="mb-3 form-check">
                    <input asp-for="ActiveInactive" class="form-check-input" />
                    <label asp-for="ActiveInactive" class="form-check-label">Active</label>
                </div>
                <div class="d-flex gap-2">
                    <button type="submit" class="btn btn-primary">Update Department</button>
                    <a asp-action="Index" class="btn btn-secondary">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    @{ await Html.RenderPartialAsync("_ValidationScriptsPartial"); }
}
```

---

## STEP 20: Add Departments Link to Navbar

Open `Views/Shared/_Layout.cshtml`, find the `<ul class="navbar-nav">` section and add:

```html
<li class="nav-item">
    <a class="nav-link text-dark" asp-controller="Department" asp-action="Index">Departments</a>
</li>
```

---

## Final Folder Structure (after all steps)

```
EmployeeManagementSystem/
├── Controllers/
│   ├── HomeController.cs
│   └── DepartmentController.cs       ← Step 16
├── Data/
│   └── ApplicationDbContext.cs       ← Step 13
├── Docs/
│   ├── Actual Setup Steps.md         ← this file
│   ├── Concepts & Q&A Notes.md
│   ├── Development Roadmap.md
│   ├── Module Breakdown.md
│   ├── Progress Tracker.md
│   ├── Project Overview & Requirements.md
│   ├── Setup Instructions.md
│   └── Technical Architecture.md
├── Migrations/                        ← Step 15 (auto-generated)
├── Models/
│   ├── Department.cs                  ← Step 11
│   ├── Employee.cs                    ← Step 12
│   └── ErrorViewModel.cs
├── Services/                          ← Step 8 (empty for now)
├── Views/
│   ├── Department/                    ← Steps 17-19
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   ├── Home/
│   ├── Shared/
│   │   └── _Layout.cshtml             ← Step 20 (updated)
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
├── appsettings.json                   ← Step 7
├── EmployeeManagementSystem.csproj
└── Program.cs                         ← Step 14
```

---

**Last Updated:** February 18, 2026  
**Status:** Modules 0, 1, 2 complete