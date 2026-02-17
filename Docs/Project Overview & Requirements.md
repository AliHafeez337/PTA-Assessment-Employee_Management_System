# Employee Management System (EMS)
## Project Overview & Requirements Documentation

---

## 1. PROJECT OVERVIEW

### 1.1 What is This Project?
This is a **web-based Employee Management System** that helps organizations manage their employees and departments. Think of it as a digital filing system where you can:
- Add, edit, and remove departments
- Add, edit, and remove employees
- Upload many employees at once using Excel/CSV files
- See statistics about your organization

### 1.2 Why Are We Building This?
To demonstrate proficiency in:
- ASP.NET MVC (a framework for building web applications)
- Database management with SQL Server
- Frontend interactivity with jQuery/JavaScript
- File upload and processing
- CRUD operations (Create, Read, Update, Delete)

---

## 2. FUNCTIONAL REQUIREMENTS

### 2.1 Department Management Module

**What it does:** Manages different departments in the organization (e.g., HR, IT, Finance)

#### Features:

**A. Add Department**
- User fills a form with:
  - Department Name (mandatory)
  - Department Code (mandatory)
  - Active/Inactive status (checkbox)
- System saves the department to database

**B. Edit Department**
- User can click "Edit" button on any department
- User can update:
  - Department Name
  - Department Code
  - Active/Inactive status
- System updates the database

**C. Delete Department**
- User can click "Delete" button
- **Important:** This is a "soft delete" - the department still exists in database but is hidden from users
- This is important because we don't want to lose historical data

**D. Department List Page**
- Shows all departments in a table format
- Columns displayed:
  - Department Name
  - Department Code
  - Active/Inactive status
  - Created Date
  - Actions (Edit/Delete buttons)

---

### 2.2 Employee Management Module

**What it does:** Manages employee information

#### Employee Information Fields:
- **Name** (mandatory)
- **Email** (mandatory, must be unique - no two employees can have same email)
- **Salary** (mandatory, must be a number)
- **Department** (dropdown to select from existing departments)
- **Joining Date** (mandatory)

#### Features:

**A. Add Employee**
- User clicks "Add Employee" button
- A **popup window** appears (not a new page)
- User fills in employee details
- System validates and saves

**B. Edit Employee**
- User clicks "Edit" button on any employee
- Popup appears with existing data
- User updates information
- System saves changes

**C. Delete Employee**
- User clicks "Delete" button
- Employee is permanently removed from database

**D. Employee List Page**
- Shows all employees in a table
- Columns displayed:
  - Name
  - Email
  - Salary
  - Department Name
  - Joining Date
  - Actions (Edit/Delete buttons)

**E. Search and Filter**
- **Search Box:** Type employee name to filter results
- **Department Filter:** Dropdown to show only employees from selected department

---

### 2.3 Bulk Employee Upload Module

**What it does:** Allows uploading many employees at once using Excel or CSV file

#### File Format Required:
```
Name, Email, Salary, Department Name, Joining Date
John Doe, john@example.com, 50000, IT, 2024-01-15
Jane Smith, jane@example.com, 60000, HR, 2024-02-01
```

#### Upload Process:
1. User selects CSV/Excel file
2. User clicks "Upload" button
3. System reads the file row by row
4. System validates each row
5. System inserts valid records
6. **If department doesn't exist:** System automatically creates it
7. System shows summary report

#### Validation Rules:
- Name cannot be empty
- Email cannot be empty and must be unique
- Salary must be a valid number
- Joining Date must be a valid date

#### Upload Summary Report Shows:
- Total Records in file
- Successfully Inserted records
- Failed Records with error messages

**Error Examples:**
- "Row 3: Email already exists"
- "Row 5: Invalid salary format"
- "Row 7: Missing required field 'Name'"

---

### 2.4 Dashboard Module

**What it does:** Shows overview statistics

#### Statistics Displayed:
- **Total Employees:** Count of all employees
- **Total Departments:** Count of all departments
- **Average Salary:** Average of all employee salaries

Simple cards or boxes displaying these numbers.

---

## 3. TECHNICAL REQUIREMENTS

### 3.1 Technology Stack

**Backend:**
- **ASP.NET MVC** - The framework we'll use (either .NET Framework or .NET Core)
- **C#** - The programming language
- **SQL Server** - The database

**Frontend:**
- **HTML/CSS** - For page structure and styling
- **jQuery** - For interactive features
- **JavaScript** - For client-side logic
- **Bootstrap** (optional) - For nice-looking UI

### 3.2 jQuery/JavaScript Requirements

**Must implement:**

1. **Client-Side Validation**
   - Check email format (must contain @)
   - Check salary is numeric before submitting
   - Show error messages on the form

2. **AJAX Feature**
   - When user selects a department in employee form
   - System makes background call to server
   - System displays: "This department has X employees"
   - This happens without refreshing the page

### 3.3 Database Requirements

**Two main tables:**

**1. Departments Table**
```
Column Name       | Data Type    | Description
------------------|--------------|---------------------------
DepartmentId      | int (PK)     | Unique identifier
DepartmentName    | nvarchar     | Name of department
DepartmentCode    | nvarchar     | Short code (e.g., "IT", "HR")
ActiveInactive    | bool         | Is department active?
CreatedDate       | datetime     | When was it created?
```

**2. Employees Table**
```
Column Name    | Data Type    | Description
---------------|--------------|---------------------------
EmployeeId     | int (PK)     | Unique identifier
Name           | nvarchar     | Employee name
Email          | nvarchar     | Employee email (unique)
Salary         | decimal      | Employee salary
DepartmentId   | int (FK)     | Links to Departments table
JoiningDate    | datetime     | When employee joined
```

**Relationship:**
- One Department can have Many Employees
- Each Employee belongs to One Department

---

## 4. VALIDATION RULES

### Department Validation:
- ✅ Department Name cannot be empty
- ✅ Department Code cannot be empty

### Employee Validation:
- ✅ Name is required
- ✅ Email is required
- ✅ Email must be unique (no duplicates)
- ✅ Email must have valid format (xxx@xxx.xxx)
- ✅ Salary must be numeric
- ✅ Salary must be positive number
- ✅ Department must be selected
- ✅ Joining Date is required

---

## 5. USER INTERFACE REQUIREMENTS

### 5.1 General UI Requirements
- Clean and simple interface
- Responsive (works on different screen sizes)
- Clear navigation menu
- Success/Error messages for user actions

### 5.2 Specific Requirements
- **Employee Entry:** Must be through popup modal (not separate page)
- **Tables:** Must have Edit and Delete buttons for each row
- **Forms:** Must have clear labels and validation messages
- **Upload:** Must show progress/loading indicator

---

## 6. NON-FUNCTIONAL REQUIREMENTS

### 6.1 Code Quality
- ✅ Code must be well-commented
- ✅ Proper naming conventions (meaningful variable names)
- ✅ Code should be organized and structured
- ✅ No unused code

### 6.2 Error Handling
- ✅ Application should not crash
- ✅ User-friendly error messages
- ✅ Validation on both client and server side

### 6.3 Performance
- ✅ Application should run smoothly
- ✅ Database queries should be efficient

---

## 7. DELIVERABLES

What we need to submit:

1. **Source Code**
   - Complete ASP.NET MVC project
   - Organized folder structure

2. **Database Script**
   - SQL script to create database
   - SQL script to create tables
   - Sample data (optional)

3. **Sample Upload File**
   - Example CSV file with correct format
   - Example Excel file with correct format

4. **README File**
   - How to setup and run the project
   - Prerequisites (what software is needed)
   - Step-by-step instructions
   - Any assumptions made

---

## 8. SUCCESS CRITERIA

The project is complete when:
- ✅ All CRUD operations work for Departments
- ✅ All CRUD operations work for Employees
- ✅ Bulk upload works with validation
- ✅ Dashboard shows correct statistics
- ✅ AJAX call works on department selection
- ✅ Client-side validation works
- ✅ No critical errors or bugs
- ✅ Code is properly documented
- ✅ README is complete and clear

---

## 9. OUT OF SCOPE

What we are NOT building:
- ❌ User authentication/login system
- ❌ Role-based access control
- ❌ Employee attendance tracking
- ❌ Payroll processing
- ❌ Leave management
- ❌ Performance reviews
- ❌ Complex reporting
- ❌ Mobile app

---

**Document Version:** 1.0  
**Last Updated:** February 17, 2026  
**Status:** Draft - Awaiting Approval