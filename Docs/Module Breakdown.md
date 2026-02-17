# Employee Management System (EMS)
## Module Breakdown Document

---

## MODULE ORGANIZATION

This project is divided into **8 modules** that we'll build step by step.

---

## MODULE 0: PROJECT SETUP & FOUNDATION
**Priority:** Critical  
**Estimated Time:** 2-3 hours  
**Dependencies:** None

### What We'll Build:
- Install required software (Visual Studio, SQL Server)
- Create new ASP.NET MVC project
- Setup database connection
- Create basic folder structure
- Setup Bootstrap for styling

### Components:
1. **Development Environment Setup**
   - Install Visual Studio 2022 Community Edition
   - Install SQL Server Express
   - Install SQL Server Management Studio (SSMS)

2. **Project Creation**
   - Create new ASP.NET MVC project
   - Choose .NET Core 6.0 or later
   - Configure project settings

3. **Database Setup**
   - Create database in SQL Server
   - Setup connection string in appsettings.json
   - Test database connection

4. **Folder Structure**
   ```
   EMS/
   ├── Controllers/         (Handles user requests)
   ├── Models/             (Data structures)
   ├── Views/              (HTML pages)
   ├── wwwroot/            (CSS, JS, images)
   ├── Data/               (Database context)
   └── Services/           (Business logic)
   ```

### Deliverables:
- ✅ Working Visual Studio project
- ✅ Database created and connected
- ✅ Project runs successfully (shows default page)

---

## MODULE 1: DATABASE DESIGN & ENTITY FRAMEWORK SETUP
**Priority:** Critical  
**Estimated Time:** 2 hours  
**Dependencies:** Module 0

### What We'll Build:
- Create database tables
- Setup Entity Framework Core
- Create Model classes
- Create Database Context
- Apply migrations

### Components:

1. **Model Classes (C# Classes)**
   ```
   Department.cs
   - DepartmentId (int)
   - DepartmentName (string)
   - DepartmentCode (string)
   - ActiveInactive (bool)
   - CreatedDate (DateTime)
   - Employees (List) - relationship

   Employee.cs
   - EmployeeId (int)
   - Name (string)
   - Email (string)
   - Salary (decimal)
   - DepartmentId (int)
   - JoiningDate (DateTime)
   - Department - relationship
   ```

2. **Database Context (ApplicationDbContext.cs)**
   - Configure Entity Framework
   - Define DbSet for Departments
   - Define DbSet for Employees
   - Configure relationships

3. **Database Migration**
   - Create migration files
   - Update database with tables

### Deliverables:
- ✅ Department and Employee tables created in SQL Server
- ✅ Entity Framework configured
- ✅ Can query data using C# code

---

## MODULE 2: DEPARTMENT MANAGEMENT
**Priority:** High  
**Estimated Time:** 4-5 hours  
**Dependencies:** Module 1

### What We'll Build:
Complete CRUD operations for Departments

### Components:

1. **Controller (DepartmentController.cs)**
   - Index() - Show list of departments
   - Create() - Show add department form
   - Create(Department) - Save new department
   - Edit(id) - Show edit form
   - Edit(Department) - Save changes
   - Delete(id) - Soft delete department

2. **Views**
   - Index.cshtml - Department list page with table
   - Create.cshtml - Add department form
   - Edit.cshtml - Edit department form
   - _DeleteConfirmation.cshtml - Delete confirmation popup

3. **Features**
   - Add new department with validation
   - Edit existing department
   - Soft delete (mark as inactive, don't remove from DB)
   - Display all departments in table
   - Show Active/Inactive status

4. **Validation**
   - Client-side validation (jQuery)
   - Server-side validation (C#)
   - Required field validation
   - Duplicate name check

### Page Flow:
```
Department List Page
    ↓
    [Add New] → Create Form → Save → Back to List
    ↓
    [Edit] → Edit Form → Update → Back to List
    ↓
    [Delete] → Confirmation → Soft Delete → Refresh List
```

### Deliverables:
- ✅ Can add departments
- ✅ Can edit departments
- ✅ Can delete departments (soft delete)
- ✅ Departments display in table
- ✅ Validation works

---

## MODULE 3: EMPLOYEE MANAGEMENT (BASIC CRUD)
**Priority:** High  
**Estimated Time:** 5-6 hours  
**Dependencies:** Module 2

### What We'll Build:
Complete CRUD operations for Employees with popup modal

### Components:

1. **Controller (EmployeeController.cs)**
   - Index() - Show list of employees
   - Create() - Return partial view for popup
   - Create(Employee) - Save new employee
   - Edit(id) - Return partial view for popup
   - Edit(Employee) - Save changes
   - Delete(id) - Delete employee
   - GetDepartmentEmployeeCount(id) - AJAX endpoint

2. **Views**
   - Index.cshtml - Employee list page
   - _CreateEmployee.cshtml - Add employee form (partial view for popup)
   - _EditEmployee.cshtml - Edit employee form (partial view for popup)

3. **Features**
   - Display employees in table
   - Add employee in popup modal
   - Edit employee in popup modal
   - Delete employee with confirmation
   - Department dropdown populated from database
   - Show department name in employee list

4. **jQuery/JavaScript**
   - Open modal on "Add Employee" button
   - Submit form via AJAX
   - Close modal after save
   - Refresh employee list without page reload
   - Client-side validation

5. **AJAX Feature**
   - When user selects department in form
   - Make AJAX call to get employee count
   - Display: "This department has X employees"

### Page Flow:
```
Employee List Page
    ↓
    [Add Employee] → Popup Opens → Fill Form → Save → Popup Closes → List Refreshes
    ↓
    [Edit] → Popup Opens → Edit Form → Update → Popup Closes → List Refreshes
    ↓
    [Delete] → Confirmation → Delete → List Refreshes
```

### Deliverables:
- ✅ Can add employees through popup
- ✅ Can edit employees through popup
- ✅ Can delete employees
- ✅ Department dropdown works
- ✅ AJAX employee count works
- ✅ Client-side validation works

---

## MODULE 4: SEARCH & FILTER FUNCTIONALITY
**Priority:** Medium  
**Estimated Time:** 2-3 hours  
**Dependencies:** Module 3

### What We'll Build:
Search and filter capabilities on Employee List page

### Components:

1. **Search by Name**
   - Search box at top of employee list
   - Filter employees as user types
   - Can use jQuery for client-side search or server-side

2. **Filter by Department**
   - Dropdown showing all departments
   - Show only employees from selected department
   - "All Departments" option to show everyone

3. **Implementation Options**
   - **Option A:** Client-side filtering (jQuery) - Fast but limited
   - **Option B:** Server-side filtering (AJAX) - Better for large data

### Features:
- Real-time search (as you type)
- Department filter dropdown
- Clear search button
- Show count: "Showing X of Y employees"

### Deliverables:
- ✅ Search box filters employees by name
- ✅ Department dropdown filters employees
- ✅ Both filters work together
- ✅ Clear filters button works

---

## MODULE 5: BULK EMPLOYEE UPLOAD
**Priority:** High  
**Estimated Time:** 6-7 hours  
**Dependencies:** Module 3

### What We'll Build:
Upload multiple employees using CSV/Excel file

### Components:

1. **Upload Page**
   - File upload button (CSV or Excel)
   - Sample file download link
   - Upload button
   - Progress indicator

2. **Controller Methods**
   - Upload() - Show upload page
   - ProcessUpload(file) - Process uploaded file
   - DownloadSample() - Download sample CSV/Excel

3. **File Processing Service**
   - Read CSV file
   - Read Excel file (using EPPlus library)
   - Parse each row
   - Validate data
   - Check for existing emails
   - Create departments if they don't exist
   - Insert valid employees
   - Collect errors

4. **Validation During Upload**
   - Check required fields
   - Validate email format
   - Validate salary is numeric
   - Validate date format
   - Check for duplicate emails in file
   - Check for duplicate emails in database

5. **Upload Results Page**
   - Total records processed
   - Successfully inserted
   - Failed records with error messages
   - Download error report (optional)

### File Format:
```csv
Name,Email,Salary,Department Name,Joining Date
John Doe,john@example.com,50000,IT,2024-01-15
Jane Smith,jane@example.com,60000,HR,2024-02-01
Bob Johnson,bob@example.com,55000,Finance,2024-01-20
```

### Upload Flow:
```
Upload Page
    ↓
Select File → Click Upload
    ↓
Processing... (show spinner)
    ↓
Results Page
    ├── Success: 45 employees added
    ├── Failed: 5 records
    └── Error Details:
        • Row 3: Duplicate email
        • Row 7: Invalid salary
        • Row 12: Missing name
```

### Deliverables:
- ✅ Can upload CSV file
- ✅ Can upload Excel file
- ✅ File is processed correctly
- ✅ Valid records are inserted
- ✅ Invalid records show errors
- ✅ Auto-create departments
- ✅ Sample file download works

---

## MODULE 6: DASHBOARD
**Priority:** Medium  
**Estimated Time:** 2-3 hours  
**Dependencies:** Module 3

### What We'll Build:
Dashboard showing key statistics

### Components:

1. **Dashboard Controller**
   - Index() - Calculate and display statistics

2. **Dashboard View**
   - Three statistic cards/boxes
   - Clean and simple design
   - Optional: Charts (using Chart.js)

3. **Statistics Calculated**
   - Total Employees (count)
   - Total Active Departments (count where ActiveInactive = true)
   - Average Salary (average of all employee salaries)

4. **Design**
   - Bootstrap cards for each statistic
   - Icons for visual appeal
   - Color coding (blue, green, orange)

### Dashboard Layout:
```
┌─────────────────────────────────────────────┐
│           EMPLOYEE MANAGEMENT SYSTEM        │
└─────────────────────────────────────────────┘

┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│   EMPLOYEES  │  │ DEPARTMENTS  │  │   AVERAGE    │
│              │  │              │  │    SALARY    │
│     150      │  │      12      │  │   $55,000    │
└──────────────┘  └──────────────┘  └──────────────┘

Recent Activities / Quick Links
```

### Deliverables:
- ✅ Dashboard displays total employees
- ✅ Dashboard displays total departments
- ✅ Dashboard displays average salary
- ✅ Dashboard is visually appealing

---

## MODULE 7: VALIDATION & ERROR HANDLING
**Priority:** High  
**Estimated Time:** 3-4 hours  
**Dependencies:** All previous modules

### What We'll Build:
Comprehensive validation and error handling

### Components:

1. **Client-Side Validation (jQuery)**
   - Email format validation
   - Salary numeric validation
   - Required field validation
   - Date format validation
   - Show error messages below fields

2. **Server-Side Validation (C#)**
   - Data annotation validation
   - Custom validation logic
   - Model state validation
   - Database constraint validation

3. **Error Handling**
   - Try-catch blocks in controllers
   - Custom error pages
   - User-friendly error messages
   - Logging errors (optional)

4. **User Feedback**
   - Success messages (green alerts)
   - Error messages (red alerts)
   - Warning messages (yellow alerts)
   - Loading indicators

### Validation Rules Implementation:

**Department:**
```
- DepartmentName: Required, Max 100 characters
- DepartmentCode: Required, Max 20 characters
- No duplicate department names
```

**Employee:**
```
- Name: Required, Max 100 characters
- Email: Required, Valid format, Unique
- Salary: Required, Numeric, Greater than 0
- DepartmentId: Required, Must exist
- JoiningDate: Required, Valid date, Not future date
```

### Deliverables:
- ✅ All forms have client-side validation
- ✅ All forms have server-side validation
- ✅ Error messages are clear and helpful
- ✅ Success messages show after actions
- ✅ Application doesn't crash on errors

---

## MODULE 8: FINAL POLISH & TESTING
**Priority:** Medium  
**Estimated Time:** 3-4 hours  
**Dependencies:** All previous modules

### What We'll Build:
Final touches and comprehensive testing

### Components:

1. **Navigation Menu**
   - Home (Dashboard)
   - Departments
   - Employees
   - Upload Employees
   - Clean Bootstrap navbar

2. **UI Improvements**
   - Consistent styling across all pages
   - Responsive design (mobile-friendly)
   - Loading spinners
   - Confirmation dialogs
   - Better table styling

3. **Code Cleanup**
   - Add comments to all code
   - Remove unused code
   - Consistent naming conventions
   - Code formatting

4. **Testing Checklist**
   - Add department - success case
   - Add department - validation errors
   - Edit department
   - Delete department
   - Add employee - success case
   - Add employee - validation errors
   - Edit employee
   - Delete employee
   - Search employees
   - Filter employees
   - Upload valid CSV
   - Upload CSV with errors
   - Dashboard statistics accuracy
   - AJAX employee count

5. **Documentation**
   - README.md file
   - Code comments
   - Database script
   - Sample CSV file

### Deliverables:
- ✅ All features tested and working
- ✅ UI is polished and professional
- ✅ Code is well-commented
- ✅ Documentation is complete
- ✅ Sample files are provided

---

## DEVELOPMENT ORDER

**Recommended sequence:**

1. **Week 1:**
   - Module 0: Project Setup (Day 1)
   - Module 1: Database Setup (Day 2)
   - Module 2: Department Management (Day 3-4)

2. **Week 2:**
   - Module 3: Employee Management (Day 1-2)
   - Module 4: Search & Filter (Day 3)
   - Module 6: Dashboard (Day 4)

3. **Week 3:**
   - Module 5: Bulk Upload (Day 1-2)
   - Module 7: Validation (Day 3)
   - Module 8: Final Polish (Day 4)

---

## EFFORT ESTIMATION

| Module | Complexity | Time Estimate |
|--------|-----------|---------------|
| Module 0 | Easy | 2-3 hours |
| Module 1 | Easy | 2 hours |
| Module 2 | Medium | 4-5 hours |
| Module 3 | Medium | 5-6 hours |
| Module 4 | Easy | 2-3 hours |
| Module 5 | Hard | 6-7 hours |
| Module 6 | Easy | 2-3 hours |
| Module 7 | Medium | 3-4 hours |
| Module 8 | Easy | 3-4 hours |
| **Total** | | **~30-37 hours** |

---

**Document Version:** 1.0  
**Last Updated:** February 17, 2026  
**Status:** Draft - Awaiting Approval