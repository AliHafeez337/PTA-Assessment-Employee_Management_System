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
- [✅] Implement soft delete (ActiveInactive = false)
- [✅] Add client-side validation (jQuery via _ValidationScriptsPartial)
- [✅] Add server-side validation (ModelState + duplicate name check)
- [✅] Style pages with Bootstrap (badges, striped table, alerts)
- [✅] Test all CRUD operations

#### Deliverables:
- ✅ Department list shows all departments (Active + Inactive) with color-coded badges
- ✅ Can add new departments
- ✅ Can edit departments
- ✅ Can soft delete departments (marked Inactive, not removed from DB)
- ✅ Duplicate name validation works (client + server side)
- ✅ Success messages display after every action
- ✅ Navbar updated with Departments link

---

## PHASE 2: EMPLOYEE MANAGEMENT (Week 2)

### MODULE 3: Employee CRUD with Popup
**Timeline:** Day 1-2 (5-6 hours)  
**Status:** 🔴 Not Started  
**Prerequisites:** Module 2

#### Tasks:

**Day 1: Employee List & Add**
- [ ] Create EmployeeController
- [ ] Implement Index action
- [ ] Create Index.cshtml (employee list with table)
- [ ] Create _CreateEmployee.cshtml (partial view for popup)
- [ ] Add "Add Employee" button that opens modal
- [ ] Populate department dropdown
- [ ] Implement Create POST action
- [ ] Add jQuery to handle modal open/close
- [ ] Add jQuery to submit form via AJAX
- [ ] Refresh list after adding employee
- [ ] Test: Can add employees through popup

**Day 2: Edit, Delete, AJAX**
- [ ] Create _EditEmployee.cshtml (partial view)
- [ ] Implement Edit GET and POST actions
- [ ] Add Edit functionality with popup
- [ ] Implement Delete action with confirmation
- [ ] Create GetDepartmentEmployeeCount action (AJAX)
- [ ] Add jQuery to call AJAX on department selection
- [ ] Display employee count dynamically
- [ ] Add client-side validation
- [ ] Test all operations

#### Deliverables:
- [ ] Employee list displays correctly
- [ ] Can add employees via popup
- [ ] Can edit employees via popup
- [ ] Can delete employees
- [ ] Department dropdown populated
- [ ] AJAX employee count works
- [ ] Validation working

#### Approval Required: ✋ YES

---

### MODULE 4: Search & Filter
**Timeline:** Day 3 (2-3 hours)  
**Status:** 🔴 Not Started  
**Prerequisites:** Module 3

#### Tasks:
- [ ] Add search textbox to Index.cshtml
- [ ] Add department filter dropdown
- [ ] Implement search functionality
- [ ] Implement filter functionality
- [ ] Make search and filter work together
- [ ] Add "Clear filters" button
- [ ] Show result count
- [ ] Test with various search terms
- [ ] Test filter combinations

#### Deliverables:
- [ ] Search by name works
- [ ] Filter by department works
- [ ] Both work together
- [ ] Clear button works
- [ ] Result count displays

#### Approval Required: ✋ YES

---

### MODULE 6: Dashboard
**Timeline:** Day 4 (2-3 hours)  
**Status:** 🔴 Not Started  
**Prerequisites:** Module 3

#### Tasks:
- [ ] Create/Update HomeController with statistics
- [ ] Calculate total employees
- [ ] Calculate total active departments
- [ ] Calculate average salary
- [ ] Create Index.cshtml with stat cards
- [ ] Style dashboard with Bootstrap cards
- [ ] Add icons (optional)
- [ ] Test statistics accuracy

#### Deliverables:
- [ ] Dashboard displays total employees
- [ ] Dashboard displays total departments
- [ ] Dashboard displays average salary
- [ ] Dashboard looks professional

#### Approval Required: ✋ YES

---

## PHASE 3: ADVANCED FEATURES (Week 3)

### MODULE 5: Bulk Employee Upload
**Timeline:** Day 1-2 (6-7 hours)  
**Status:** 🔴 Not Started  
**Prerequisites:** Module 3

#### Tasks:

**Day 1: Upload Setup & File Reading**
- [ ] Create Upload.cshtml view
- [ ] Add file upload input
- [ ] Create sample CSV file
- [ ] Create sample Excel file
- [ ] Install EPPlus NuGet package
- [ ] Create FileUploadService class
- [ ] Implement CSV reading logic
- [ ] Implement Excel reading logic
- [ ] Test file reading

**Day 2: Processing & Validation**
- [ ] Implement row validation logic
- [ ] Check required fields
- [ ] Validate email format
- [ ] Validate salary is numeric
- [ ] Validate date format
- [ ] Check for duplicate emails in file
- [ ] Check for duplicate emails in database
- [ ] Implement auto-create department logic
- [ ] Implement bulk insert logic
- [ ] Create upload result summary
- [ ] Create error list display
- [ ] Add download sample file link
- [ ] Test with valid file
- [ ] Test with invalid file
- [ ] Test with large file (1000+ records)

#### Approval Required: ✋ YES

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
- [ ] Add navigation menu
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

- Module 0: 🟢 Complete (100%)
- Module 1: 🟢 Complete (100%)
- Module 2: 🔴 Not Started (0%)
- Module 3: 🔴 Not Started (0%)
- Module 4: 🔴 Not Started (0%)
- Module 5: 🔴 Not Started (0%)
- Module 6: 🔴 Not Started (0%)
- Module 7: 🔴 Not Started (0%)
- Module 8: 🔴 Not Started (0%)

**Overall: ~22% complete**

---

## ESTIMATED TIMELINE

**Optimistic:** 3 weeks  
**Realistic:** 4-5 weeks  
**Pessimistic:** 6 weeks

---

**Document Version:** 2.0  
**Last Updated:** February 18, 2026  
**Status:** In Progress