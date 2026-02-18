# Employee Management System (EMS)
## Development Roadmap

---

## DEVELOPMENT APPROACH

We'll build this project **incrementally** - one module at a time. Each module will be:
1. ✅ Approved before starting
2. 📝 Fully documented with commented code
3. ✅ Tested before moving to next module
4. 📊 Progress tracked

---

## PHASE 1: FOUNDATION (Week 1)

### MODULE 0: Project Setup & Foundation
**Timeline:** Day 1 (2-3 hours)
**Status:** 🟢 Completed — February 17, 2026

#### Tasks:
- [✅] Install Visual Studio / VS Code + .NET SDK
- [✅] Install Docker Desktop
- [✅] Setup SQL Server 2019 in Docker
- [✅] Create new ASP.NET Core MVC project
- [✅] Configure project settings
- [✅] Create database in DBeaver
- [✅] Setup connection string
- [✅] Test database connection
- [✅] Setup Bootstrap and jQuery
- [✅] Create basic folder structure
- [✅] Test run application

#### Deliverables:
- ✅ Working VS Code project (.NET 10)
- ✅ Connected to SQL Server (Docker) via DBeaver
- ✅ Application runs on http://localhost:5099

---

### MODULE 1: Database Design & Entity Framework
**Timeline:** Day 2 (2 hours)
**Status:** 🟢 Completed — February 18, 2026

#### Tasks:
- [✅] Create Department.cs model class
- [✅] Create Employee.cs model class
- [✅] Create ApplicationDbContext.cs
- [✅] Configure Entity Framework in Program.cs
- [✅] Define relationships between models
- [✅] Install EF Core NuGet packages (v8.0.0)
- [✅] Install dotnet-ef CLI tool globally
- [✅] Create initial migration (InitialCreate)
- [✅] Update database (create tables)
- [✅] Verify tables created in DBeaver

#### Deliverables:
- ✅ Departments and Employees tables in SQL Server
- ✅ Entity Framework configured and connected
- ✅ Relationships and unique indexes defined

---

### MODULE 2: Department Management
**Timeline:** Day 3-4 (4-5 hours)
**Status:** 🟢 Completed — February 18, 2026

#### Tasks:
- [✅] Create DepartmentController
- [✅] Implement Index action (list all departments)
- [✅] Create Index.cshtml view
- [✅] Implement Create GET action
- [✅] Create Create.cshtml view (add form)
- [✅] Implement Create POST action
- [✅] Implement Edit GET action
- [✅] Create Edit.cshtml view
- [✅] Implement Edit POST action
- [✅] Implement hard delete with FK guard
- [✅] Add client-side validation (jQuery via _ValidationScriptsPartial)
- [✅] Add server-side validation (ModelState + duplicate name check)
- [✅] Style pages with Bootstrap
- [✅] Test all CRUD operations

#### Deliverables:
- ✅ Department list shows all departments (Active + Inactive) with color-coded badges
- ✅ Can add, edit, and delete departments
- ✅ Delete blocked with friendly error if employees are assigned
- ✅ Duplicate name validation works (client + server side)
- ✅ Success/error messages display after every action
- ✅ Navbar updated with Departments link

---

## PHASE 2: EMPLOYEE MANAGEMENT (Week 2)

### MODULE 3: Employee CRUD with Popup
**Timeline:** Day 1-2 (5-6 hours)
**Status:** 🟢 Completed — February 18, 2026

#### Tasks:
- [✅] Create EmployeeController
- [✅] Implement Index action (with .Include() for Department)
- [✅] Create Index.cshtml (employee list with table)
- [✅] Create _CreateEmployee.cshtml (partial view for popup)
- [✅] Add "Add Employee" button that opens modal
- [✅] Populate department dropdown (active only)
- [✅] Implement Create POST action (with duplicate email check)
- [✅] Add jQuery to handle modal open/close
- [✅] Add jQuery to submit form via AJAX
- [✅] Refresh list after saving employee
- [✅] Create _EditEmployee.cshtml (partial view)
- [✅] Implement Edit GET and POST actions
- [✅] Implement Delete action (AJAX with antiforgery token)
- [✅] Add @Html.AntiForgeryToken() to Index page
- [✅] Create GetDepartmentEmployeeCount AJAX endpoint
- [✅] Display employee count on department selection
- [✅] Add client-side validation (_ValidationScriptsPartial)
- [✅] Test all operations

#### Deliverables:
- ✅ Employee list displays with department names
- ✅ Add/Edit employees via popup modal (AJAX, no page reload)
- ✅ Delete employees with confirmation (row fades out)
- ✅ Department dropdown shows live employee count
- ✅ Validation working (client + server side)

---

### MODULE 4: Search & Filter
**Timeline:** Day 3 (2-3 hours)
**Status:** 🟢 Completed — February 18, 2026

#### Tasks:
- [✅] Add search textbox to Employee Index.cshtml
- [✅] Add department filter dropdown
- [✅] Update Index action to accept searchName and departmentId params
- [✅] Implement server-side filtering via AsQueryable()
- [✅] Make search and filter work together (combined query)
- [✅] Pass filter values back via ViewBag (inputs stay filled)
- [✅] Add "Clear filters" link
- [✅] Show result count (changes wording when filters active)
- [✅] Test all combinations

#### Deliverables:
- ✅ Search by name works
- ✅ Filter by department works
- ✅ Both work together
- ✅ Clear link resets everything
- ✅ Result count displays correctly

---

### MODULE 6: Dashboard
**Timeline:** Day 4 (2-3 hours)
**Status:** 🟢 Completed — February 18, 2026

#### Tasks:
- [✅] Create DashboardViewModel.cs
- [✅] Update HomeController to inject ApplicationDbContext
- [✅] Calculate TotalEmployees (CountAsync)
- [✅] Calculate TotalActiveDepartments (CountAsync with filter)
- [✅] Calculate AverageSalary (AnyAsync guard + AverageAsync)
- [✅] Replace Views/Home/Index.cshtml with dashboard layout
- [✅] Add three stat cards (Employees, Departments, Average Salary)
- [✅] Add quick links to Employees and Departments
- [✅] Verify navbar Home link
- [✅] Test statistics accuracy

#### Deliverables:
- ✅ Dashboard displays total employees, active departments, average salary
- ✅ Stats are live from the database
- ✅ Quick links to Employees and Departments sections
- ✅ Clean Bootstrap card layout with icons

---

## PHASE 3: ADVANCED FEATURES (Week 3)

### MODULE 5: Bulk Employee Upload
**Timeline:** Day 1-2 (6-7 hours)
**Status:** 🟢 Completed — February 18, 2026
**Prerequisites:** Module 3 ✅

#### Tasks:

**Day 1: Upload Setup & File Reading**
- [✅] Create Upload.cshtml view
- [✅] Add file upload input (.csv and .xlsx)
- [✅] Create sample CSV file
- [✅] Install EPPlus NuGet package (v7.0.0)
- [✅] Set EPPlus LicenseContext in Program.cs
- [✅] Create FileUploadService class
- [✅] Implement CSV reading logic
- [✅] Implement Excel reading logic
- [✅] Register FileUploadService in Program.cs

**Day 2: Processing & Validation**
- [✅] Create UploadResult.cs and RowResult.cs models
- [✅] Implement row validation (name, email, salary, date)
- [✅] Check for duplicate emails within the file
- [✅] Check for duplicate emails against the database
- [✅] Implement auto-create department logic
- [✅] Implement bulk insert with in-memory cache
- [✅] Create UploadResult.cshtml (summary + row detail table)
- [✅] Add Upload GET/POST actions to EmployeeController
- [✅] Add Bulk Upload button to Employee Index page
- [✅] Test with valid file
- [✅] Test with invalid rows
- [✅] Test duplicate detection
- [✅] Test auto-create department

#### Deliverables:
- ✅ Can upload CSV and Excel files
- ✅ Valid rows are inserted, invalid rows skipped with errors
- ✅ Departments auto-created if they don't exist
- ✅ Duplicate emails caught (in-file and in-database)
- ✅ Results page shows summary cards + row-by-row detail

---

### MODULE 7: Validation & Error Handling
**Timeline:** Day 3 (3-4 hours)
**Status:** 🔴 Not Started
**Prerequisites:** All previous modules

#### Tasks:
- [ ] Review all forms for client-side validation
- [ ] Add jQuery validation to all forms
- [ ] Add data annotations to models
- [ ] Implement server-side validation in controllers
- [ ] Add try-catch blocks to all controller actions
- [ ] Create custom error page
- [ ] Add user-friendly error messages
- [ ] Add success messages for all actions
- [ ] Add loading indicators
- [ ] Test validation on all forms
- [ ] Test error scenarios

#### Approval Required: ✋ YES

---

### MODULE 8: Final Polish & Documentation
**Timeline:** Day 4 (3-4 hours)
**Status:** 🔴 Not Started
**Prerequisites:** All previous modules

#### Tasks:
- [ ] Add comments to all code files
- [ ] Format code consistently
- [ ] Remove unused code
- [ ] Consistent naming conventions
- [ ] Review and fix UI inconsistencies
- [ ] Make responsive (mobile-friendly)
- [ ] Add navigation menu improvements
- [ ] Polish table styling
- [ ] Add confirmation dialogs
- [ ] Test all features across browsers
- [ ] Create README.md
- [ ] Document setup steps
- [ ] Create database script
- [ ] Add sample files

#### Approval Required: ✋ YES

---

## OVERALL PROGRESS

| Module | Status | Progress |
|--------|--------|----------|
| Module 0: Setup | 🟢 Complete | 100% |
| Module 1: Database | 🟢 Complete | 100% |
| Module 2: Departments | 🟢 Complete | 100% |
| Module 3: Employees | 🟢 Complete | 100% |
| Module 4: Search/Filter | 🟢 Complete | 100% |
| Module 5: Bulk Upload | 🔴 Not Started | 0% |
| Module 6: Dashboard | 🟢 Complete | 100% |
| Module 7: Validation | 🔴 Not Started | 0% |
| Module 8: Final Polish | 🔴 Not Started | 0% |

**Overall: ~66% complete**

---

## ESTIMATED TIMELINE

**Optimistic:** 1 more week
**Realistic:** 1-2 more weeks
**Pessimistic:** 2-3 more weeks

---

**Document Version:** 3.0
**Last Updated:** February 18, 2026
**Status:** In Progress — Modules 5, 7, 8 remaining