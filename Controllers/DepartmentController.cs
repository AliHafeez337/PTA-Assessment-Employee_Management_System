using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentController> _logger;

        // ILogger is injected automatically by ASP.NET — used to log errors to the console/output
        public DepartmentController(ApplicationDbContext context, ILogger<DepartmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ---------------------------------------------------------------
        // INDEX — Show list of all departments
        // URL: GET /Department
        // ---------------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            try
            {
                var departments = await _context.Departments
                    .OrderBy(d => d.DepartmentName)
                    .ToListAsync();

                return View(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading departments list");
                TempData["ErrorMessage"] = "An error occurred while loading departments. Please try again.";
                return View(new List<Department>());
            }
        }

        // ---------------------------------------------------------------
        // CREATE (GET) — Show the empty "Add Department" form
        // URL: GET /Department/Create
        // ---------------------------------------------------------------
        public IActionResult Create()
        {
            return View(new Department());
        }

        // ---------------------------------------------------------------
        // CREATE (POST) — Receive form data and save to database
        // URL: POST /Department/Create
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            // Duplicate name check
            bool nameExists = await _context.Departments
                .AnyAsync(d => d.DepartmentName == department.DepartmentName);

            if (nameExists)
                ModelState.AddModelError("DepartmentName", "A department with this name already exists.");

            if (ModelState.IsValid)
            {
                try
                {
                    department.CreatedDate = DateTime.Now;
                    _context.Departments.Add(department);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating department '{Name}'", department.DepartmentName);
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                }
            }

            return View(department);
        }

        // ---------------------------------------------------------------
        // EDIT (GET) — Show the "Edit Department" form pre-filled with data
        // URL: GET /Department/Edit/5
        // ---------------------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FindAsync(id);

            if (department == null) return NotFound();

            return View(department);
        }

        // ---------------------------------------------------------------
        // EDIT (POST) — Save updated department data
        // URL: POST /Department/Edit/5
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.DepartmentId) return NotFound();

            // Duplicate name check — exclude current department
            bool nameExists = await _context.Departments
                .AnyAsync(d => d.DepartmentName == department.DepartmentName
                            && d.DepartmentId != id);

            if (nameExists)
                ModelState.AddModelError("DepartmentName", "A department with this name already exists.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Departments.Update(department);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating department ID {Id}", id);
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                }
            }

            return View(department);
        }

        // ---------------------------------------------------------------
        // DELETE (POST) — Hard delete with FK guard
        // URL: POST /Department/Delete/5
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);

                if (department == null) return NotFound();

                // Block delete if employees are assigned
                bool hasEmployees = await _context.Employees
                    .AnyAsync(e => e.DepartmentId == id);

                if (hasEmployees)
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{department.DepartmentName}' " +
                                              $"because it has employees assigned to it. " +
                                              $"Please reassign or delete those employees first.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department ID {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}