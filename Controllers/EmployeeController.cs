using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Employee/Index
        // Fetches all employees with their department info
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)  // Eager load department
                .OrderBy(e => e.Name)
                .ToListAsync();

            return View(employees);
        }

        // GET: /Employee/Create
        // Returns the partial view (popup form) for adding an employee
        public async Task<IActionResult> Create()
        {
            // Populate department dropdown
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)  // Only active departments
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_CreateEmployee", new Employee());
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            // Check for duplicate email
            bool emailExists = await _context.Employees
                .AnyAsync(e => e.Email == employee.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "An employee with this email already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                // Return JSON success so AJAX can handle it
                return Json(new { success = true, message = "Employee added successfully." });
            }

            // If validation failed, re-populate dropdown and return partial view with errors
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_CreateEmployee", employee);
        }

        // GET: /Employee/Edit/5
        // Returns the partial view (popup form) pre-filled with employee data
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_EditEmployee", employee);
        }

        // POST: /Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            // Check for duplicate email - exclude current employee
            bool emailExists = await _context.Employees
                .AnyAsync(e => e.Email == employee.Email && e.EmployeeId != employee.EmployeeId);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "An employee with this email already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Employee updated successfully." });
            }

            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_EditEmployee", employee);
        }

        // POST: /Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return Json(new { success = false, message = "Employee not found." });
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Employee deleted successfully." });
        }

        // GET: /Employee/GetDepartmentEmployeeCount/5
        // AJAX endpoint - returns how many employees are in a department
        public async Task<IActionResult> GetDepartmentEmployeeCount(int departmentId)
        {
            int count = await _context.Employees
                .CountAsync(e => e.DepartmentId == departmentId);

            return Json(new { count = count });
        }
    }
}