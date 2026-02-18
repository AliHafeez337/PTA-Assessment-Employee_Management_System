using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        // _context gives us access to the database through Entity Framework
        // The underscore prefix is a C# convention for private fields
        private readonly ApplicationDbContext _context;

        // Constructor: ASP.NET automatically injects the DbContext we registered in Program.cs
        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------------------------------------------------------------
        // INDEX — Show list of all active departments
        // URL: GET /Department  or  GET /Department/Index
        // ---------------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            // Fetch only active departments, ordered by name
            // "async/await" means: wait for the DB to respond without freezing the app
            var departments = await _context.Departments
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(departments);   // Pass the list to Index.cshtml
        }

        // ---------------------------------------------------------------
        // CREATE (GET) — Show the empty "Add Department" form
        // URL: GET /Department/Create
        // ---------------------------------------------------------------
        public IActionResult Create()
        {
            // Pass a new Department object so the view knows the defaults
            // (e.g. ActiveInactive = true → checkbox renders as checked)
            return View(new Department());
        }

        // ---------------------------------------------------------------
        // CREATE (POST) — Receive form data and save to database
        // URL: POST /Department/Create
        // [HttpPost] means this action only runs when the form is submitted
        // [ValidateAntiForgeryToken] protects against cross-site request forgery attacks
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            // Check if a department with the same name already exists
            bool nameExists = await _context.Departments
                .AnyAsync(d => d.DepartmentName == department.DepartmentName);

            if (nameExists)
            {
                // Add a custom error to ModelState so it shows on the form
                ModelState.AddModelError("DepartmentName", "A department with this name already exists.");
            }

            // ModelState.IsValid checks all data annotation rules (Required, StringLength etc.)
            if (ModelState.IsValid)
            {
                department.CreatedDate = DateTime.Now;  // Set creation timestamp
                _context.Departments.Add(department);   // Stage the new record
                await _context.SaveChangesAsync();       // Commit to database

                // TempData persists a message for exactly one redirect (shown on Index)
                TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // If validation failed, return the same form with error messages
            return View(department);
        }

        // ---------------------------------------------------------------
        // EDIT (GET) — Show the "Edit Department" form pre-filled with data
        // URL: GET /Department/Edit/5
        // ---------------------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            // id? means it's nullable — handle the case where no id was provided
            if (id == null)
                return NotFound();

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound();

            return View(department);    // Pass existing data to pre-fill the form
        }

        // ---------------------------------------------------------------
        // EDIT (POST) — Save updated department data
        // URL: POST /Department/Edit/5
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.DepartmentId)
                return NotFound();

            // Check for duplicate name — but exclude the current department itself
            bool nameExists = await _context.Departments
                .AnyAsync(d => d.DepartmentName == department.DepartmentName
                            && d.DepartmentId != id);

            if (nameExists)
            {
                ModelState.AddModelError("DepartmentName", "A department with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);    // Stage the update
                await _context.SaveChangesAsync();           // Commit to database

                TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // ---------------------------------------------------------------
        // DELETE (POST) — Hard delete: permanently remove from database
        // URL: POST /Department/Delete/5
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound();

            // Hard delete: permanently remove the record from the DB
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Department '{department.DepartmentName}' deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}