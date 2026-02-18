# EMS Development Session Log
## Date: February 18, 2026

---

## MODULES COMPLETED THIS SESSION

| Module | Status |
|--------|--------|
| Module 1: Database & Entity Framework | ✅ Completed |
| Module 2: Department Management | ✅ Completed |
| Module 3: Employee Management | ✅ Completed |
| Module 4: Search & Filter | ✅ Completed |
| Module 5: Bulk Employee Upload | ✅ Completed |
| Module 6: Dashboard | ✅ Completed |

---

## MODULE 1 — DATABASE & ENTITY FRAMEWORK

### Files Created:
- `Models/Department.cs`
- `Models/Employee.cs`
- `Data/ApplicationDbContext.cs`
- `Program.cs` (updated)
- `Migrations/` (auto-generated)

### What Was Done:
- Created `Department` and `Employee` model classes with data annotations
- Created `ApplicationDbContext` with `DbSet<Department>` and `DbSet<Employee>`
- Configured EF Core in `Program.cs` with SQL Server connection string
- Defined one-to-many relationship (Department → Employees) in `OnModelCreating`
- Added unique index on `DepartmentName`
- Set cascade delete to `Restrict` (can't delete department if employees exist)
- Installed EF Core packages (v8.0.0): `SqlServer`, `Tools`, `Design`
- Ran `dotnet ef migrations add InitialCreate` and `dotnet ef database update`
- Verified `Departments` and `Employees` tables in DBeaver

### NuGet Packages:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
```

---

## MODULE 2 — DEPARTMENT MANAGEMENT

### Files Created:
- `Controllers/DepartmentController.cs`
- `Views/Department/Index.cshtml`
- `Views/Department/Create.cshtml`
- `Views/Department/Edit.cshtml`
- `Views/Shared/_Layout.cshtml` (updated — Departments nav link added)

### What Was Done:
- Full CRUD: Index, Create (GET/POST), Edit (GET/POST), Delete (POST)
- Server-side validation: `ModelState` + duplicate name check via `AnyAsync`
- Client-side validation: `_ValidationScriptsPartial` + `asp-validation-for` tag helpers
- `TempData["SuccessMessage"]` and `TempData["ErrorMessage"]` for user feedback
- Bootstrap badges for Active (green) / Inactive (red) status
- **Hard delete** — `_context.Departments.Remove()` (changed from soft delete per preference)
- **FK guard on delete** — checks `AnyAsync` for employees before removing; shows friendly error if blocked
- Shows all departments (Active + Inactive) in the list

### Key Decisions:
- Hard delete chosen over soft delete
- All departments shown regardless of status, with color-coded badges
- Delete blocked with `TempData["ErrorMessage"]` if department has employees

### Bugs Fixed:
| Bug | Fix |
|-----|-----|
| Departments not showing after create | `ActiveInactive` was defaulting to `false` — fixed by passing `new Department()` from GET action |
| Department delete crashing | FK constraint error — added employee count check before delete |

---

## MODULE 3 — EMPLOYEE MANAGEMENT (AJAX POPUP)

### Files Created:
- `Controllers/EmployeeController.cs`
- `Views/Employee/Index.cshtml`
- `Views/Employee/_CreateEmployee.cshtml`
- `Views/Employee/_EditEmployee.cshtml`
- `Views/Shared/_Layout.cshtml` (updated — Employees nav link added)

### Migrations Added:
```bash
dotnet ef migrations add NullableJoiningDate
dotnet ef database update
```

### What Was Done:
- Employee list with `.Include(e => e.Department)` for eager loading
- Add/Edit via Bootstrap modal popup loaded via `$.get()` (partial views)
- AJAX form submission — no page reload on save
- Delete via `$.post()` with antiforgery token — row fades out on success
- `GetDepartmentEmployeeCount` AJAX endpoint — shows live count when department selected
- `_ValidationScriptsPartial` included on Index page (needed for dynamically loaded forms)
- `@Html.AntiForgeryToken()` added to Index page (needed for AJAX delete)
- `[Range(1, int.MaxValue)]` on `DepartmentId` instead of `[Required]` (int is never null)
- `DateTime?` (nullable) for `JoiningDate` to fix date picker bug

### AJAX Flow:
```
openCreateModal()
  → $.get('/Employee/Create')
    → Controller returns PartialView("_CreateEmployee")
      → jQuery injects HTML into #modalContent
        → Modal opens

Form submit (bindFormSubmit)
  → e.preventDefault() first (prevents page reload)
  → form.valid() check
  → $.ajax POST with form.serialize()
    → Success: close modal, show alert, reload page
    → Validation error: re-inject partial view with errors into modal
```

### Bugs Fixed:
| Bug | Fix |
|-----|-----|
| Page refreshing instead of AJAX | Switched from `onsubmit` inline to jQuery `.on('submit')` with `e.preventDefault()` first |
| `$.validator is undefined` | Added `@await Html.PartialAsync("_ValidationScriptsPartial")` to Index Scripts section |
| Build error: `RenderPartialAsync` in section | Changed to `Html.PartialAsync` (returns value; `RenderPartialAsync` returns void) |
| Date picker stuck on year 0001 | Made `JoiningDate` nullable (`DateTime?`) + new migration |
| `[Required]` on `DepartmentId` always passing | Replaced with `[Range(1, int.MaxValue)]` |
| Employee delete returning 400 | No antiforgery token on page — added `@Html.AntiForgeryToken()` to Index.cshtml |
| Department delete FK crash | Added employee count check; shows `TempData["ErrorMessage"]` if blocked |

---

## MODULE 4 — SEARCH & FILTER

### Files Modified:
- `Controllers/EmployeeController.cs` — Index action updated
- `Views/Employee/Index.cshtml` — search/filter bar added

### What Was Done:
- Search box (by name) and department dropdown filter added above employee table
- Server-side filtering using `AsQueryable()` with chained `.Where()` calls
- Both filters combine — name + department work together
- `ViewBag.SearchName` and `ViewBag.SelectedDepartmentId` passed back so inputs stay filled
- "Clear" button is a plain `<a asp-action="Index">` link — navigates to `/Employee` with no params
- Result count changes wording when filters are active
- Search form uses `method="get"` — filters appear in URL as query parameters

### How It Works:
```csharp
var query = _context.Employees.Include(e => e.Department).AsQueryable();

if (!string.IsNullOrWhiteSpace(searchName))
    query = query.Where(e => e.Name.Contains(searchName));

if (departmentId.HasValue && departmentId.Value > 0)
    query = query.Where(e => e.DepartmentId == departmentId.Value);

var employees = await query.OrderBy(e => e.Name).ToListAsync();
```

---

## MODULE 6 — DASHBOARD

### Files Created/Modified:
- `Models/DashboardViewModel.cs` — new
- `Controllers/HomeController.cs` — replaced
- `Views/Home/Index.cshtml` — replaced

### What Was Done:
- Created `DashboardViewModel` with `TotalEmployees`, `TotalActiveDepartments`, `AverageSalary`
- Replaced `HomeController` with DB-connected version using `ApplicationDbContext`
- Three Bootstrap stat cards with inline SVG icons (blue/green/yellow)
- Two quick link cards — Go to Employees, Go to Departments
- `AnyAsync()` guard before `AverageAsync()` to handle empty table

### Bug Fixed:
| Bug | Fix |
|-----|-----|
| `DefaultIfEmpty(0)` EF translation error | EF can't translate C# constant fallback to SQL — replaced with `AnyAsync()` guard |

```csharp
// Before (broken):
AverageSalary = await _context.Employees.Select(e => e.Salary).DefaultIfEmpty(0).AverageAsync()

// After (working):
AverageSalary = await _context.Employees.AnyAsync()
    ? await _context.Employees.AverageAsync(e => e.Salary)
    : 0
```

---

## MODULE 5 — BULK EMPLOYEE UPLOAD

### Files Created:
- `Models/UploadResult.cs` (`RowResult` + `UploadResult` classes)
- `Services/FileUploadService.cs`
- `Views/Employee/Upload.cshtml`
- `Views/Employee/UploadResult.cshtml`
- `sample_employees.csv` (test file — 7 valid rows, 5 broken rows)

### Files Modified:
- `Controllers/EmployeeController.cs` — Upload GET/POST actions added
- `Views/Employee/Index.cshtml` — Bulk Upload button added to header
- `Program.cs` — EPPlus license + service registration added

### NuGet Package:
```bash
dotnet add package EPPlus --version 7.0.0
```

### Program.cs additions:
```csharp
using OfficeOpenXml;
using EmployeeManagementSystem.Services;

ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
builder.Services.AddScoped<FileUploadService>();
```

### What Was Done:
- Upload page with format instructions table
- Supports both `.csv` and `.xlsx`
- `FileUploadService` handles all processing:
  - `ReadCsv()` — `StreamReader`, skips header row
  - `ReadExcel()` — EPPlus, starts from row 2
  - `ProcessRowsAsync()` — validates, inserts, reports
- Validation per row: name required, email format, salary numeric, date parseable
- Duplicate email check: against database AND within the file itself
- Auto-creates department if it doesn't exist
- In-memory cache (emails list + departments list) loaded before loop — avoids N+1 queries
- `HashSet<string>` for tracking emails seen within the current file
- Row numbering uses `i + 2` (not `i + 1`) — because `i=0` maps to file row 2 after header skip
- Results page: summary cards (Total / Imported / Failed) + row-by-row detail table

### Bug Fixed:
| Bug | Fix |
|-----|-----|
| `FileUploadService` not found | Missing `using EmployeeManagementSystem.Services;` in both `EmployeeController.cs` and `Program.cs` |

### Sample CSV Structure:
```
Name,Email,Salary,Department Name,Joining Date
Ali Hafeez,ali@example.com,95000,Engineering,2024-01-15
...
```

---

## CONCEPTS LEARNED THIS SESSION

| Concept | Module | Summary |
|---------|--------|---------|
| `virtual` on navigation property | M1 | Enables EF lazy loading |
| `get; set;` | M1 | C# auto-property — compiler generates backing field |
| `modelBuilder` | M1 | EF configuration for indexes, relationships, constraints |
| `base` | M1 | Calls parent class constructor before yours |
| `readonly` + constructor assignment | M2 | Locked after construction — prevents accidental reassignment |
| `ModelState` | M2 | Dictionary of submitted values + validation errors |
| `TempData` | M2 | Survives exactly one redirect |
| `RedirectToAction(nameof(...))` | M2 | HTTP 302 + compile-safe method name |
| Antiforgery token | M2 | Prevents CSRF attacks |
| Partial view | M3 | Fragment of HTML — no layout — injected via AJAX |
| `e.preventDefault()` must be first | M3 | Prevents page reload even if later JS throws |
| `[Range]` on `int` instead of `[Required]` | M3 | `int` is never null — `[Required]` always passes |
| `DateTime?` nullable | M3 | Fixes date picker defaulting to year 0001 |
| `$.validator.unobtrusive.parse()` | M3 | Re-wires validation for dynamically injected forms |
| `.Include()` eager loading | M3 | Loads related entity in same SQL query |
| `return Json(...)` | M3 | Returns JSON to AJAX caller instead of HTML |
| `Html.PartialAsync` vs `RenderPartialAsync` | M3 | `PartialAsync` usable inside `@section`; `RenderPartialAsync` returns void |
| `method="get"` on search form | M4 | Filters go into URL — bookmarkable, back-button friendly |
| `AsQueryable()` + deferred execution | M4 | Build SQL query in memory; DB hit only on `ToListAsync()` |
| ViewModel | M6 | Plain C# class shaped for view's needs — not a DB model |
| `AnyAsync()` guard before `AverageAsync()` | M6 | `AverageAsync()` throws on empty table |
| `DefaultIfEmpty(0)` EF limitation | M6 | C# constant fallback can't be translated to SQL |
| `IFormFile` | M5 | ASP.NET interface for uploaded files |
| `enctype="multipart/form-data"` | M5 | Required on file upload forms |
| N+1 query problem | M5 | Load data once before loop; check in memory per row |
| `i + 2` row numbering | M5 | Header row already skipped — `i=0` maps to file row 2 |
| `HashSet<string>` | M5 | O(1) lookup — faster than List for existence checks |
| `AddScoped<T>()` + `[FromServices]` | M5 | DI registration + per-action injection |

---

## MIGRATIONS ADDED THIS SESSION

| Migration | Reason |
|-----------|--------|
| `InitialCreate` | Created Departments and Employees tables |
| `NullableJoiningDate` | Changed `JoiningDate` from `DateTime` to `DateTime?` |

---

## BUGS ENCOUNTERED & FIXED THIS SESSION

| # | Module | Bug | Fix |
|---|--------|-----|-----|
| 1 | M2 | Departments not showing after create | Pass `new Department()` from GET action so `ActiveInactive` defaults to `true` |
| 2 | M2 | Department delete FK crash | Check employee count before delete; show friendly error |
| 3 | M3 | Page refreshing on form submit | Switch to jQuery `.on('submit')` with `e.preventDefault()` first |
| 4 | M3 | `$.validator is undefined` | Add `_ValidationScriptsPartial` to Employee Index Scripts section |
| 5 | M3 | Build error: `RenderPartialAsync` in section | Use `Html.PartialAsync` instead |
| 6 | M3 | Date picker stuck on year 0001 | Make `JoiningDate` nullable + add migration |
| 7 | M3 | `[Required]` on `DepartmentId` always passing | Replace with `[Range(1, int.MaxValue)]` |
| 8 | M3 | Employee delete returning 400 | Add `@Html.AntiForgeryToken()` to Index page |
| 9 | M6 | `DefaultIfEmpty(0)` EF translation error | Use `AnyAsync()` guard + `AverageAsync()` |
| 10 | M5 | `FileUploadService` not found | Add `using EmployeeManagementSystem.Services;` to EmployeeController + Program.cs |

---

**Session Duration:** ~5 hours
**Session Date:** February 18, 2026
**Modules Remaining After This Session:** Module 7 (Validation), Module 8 (Final Polish)