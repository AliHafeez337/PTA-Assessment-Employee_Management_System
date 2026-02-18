using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeController> _logger;

        // Both dependencies are injected automatically by ASP.NET's DI container.
        // _context  → database access via Entity Framework
        // _logger   → writes errors to the terminal output
        public EmployeeController(ApplicationDbContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ---------------------------------------------------------------
        // INDEX – Show list of all employees with optional search/filter
        // URL: GET /Employee
        // URL: GET /Employee?searchName=Ali&departmentId=2
        // ---------------------------------------------------------------
        public async Task<IActionResult> Index(string? searchName, int? departmentId)
        {
            try
            {
                // Start with all employees (including their Department via JOIN)
                // AsQueryable() lets us chain .Where() conditions before hitting the DB
                var query = _context.Employees
                    .Include(e => e.Department)
                    .AsQueryable();

                // Filter by name if provided (case-insensitive LIKE query)
                if (!string.IsNullOrWhiteSpace(searchName))
                    query = query.Where(e => e.Name.Contains(searchName));

                // Filter by department if a valid department was selected
                if (departmentId.HasValue && departmentId.Value > 0)
                    query = query.Where(e => e.DepartmentId == departmentId.Value);

                // Execute the query — DB is not hit until ToListAsync()
                var employees = await query.OrderBy(e => e.Name).ToListAsync();

                // Populate the department dropdown (active departments only)
                ViewBag.Departments = await _context.Departments
                    .Where(d => d.ActiveInactive)
                    .OrderBy(d => d.DepartmentName)
                    .ToListAsync();

                // Pass filter values back so the search bar stays filled after submit
                ViewBag.SearchName = searchName;
                ViewBag.SelectedDepartmentId = departmentId;

                return View(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading employees list");
                TempData["ErrorMessage"] = "An error occurred while loading employees. Please try again.";
                return View(new List<Employee>());
            }
        }

        // ---------------------------------------------------------------
        // CREATE (GET) – Return the partial view used inside the modal popup
        // URL: GET /Employee/Create  (called via AJAX, not direct navigation)
        // Returns: HTML fragment (partial view), not a full page
        // ---------------------------------------------------------------
        public async Task<IActionResult> Create()
        {
            // Populate department dropdown for the form
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            // Return partial view (no layout) — jQuery injects this into the modal
            return PartialView("_CreateEmployee", new Employee());
        }

        // ---------------------------------------------------------------
        // CREATE (POST) – Validate and save new employee
        // URL: POST /Employee/Create  (submitted via AJAX)
        // Returns: JSON on success, partial view HTML on validation failure
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            // Server-side duplicate email check (not expressible as a data annotation)
            bool emailExists = await _context.Employees
                .AnyAsync(e => e.Email == employee.Email);

            if (emailExists)
                ModelState.AddModelError("Email", "An employee with this email already exists.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    // Return JSON — jQuery checks res.success to close the modal
                    return Json(new { success = true, message = "Employee added successfully." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating employee '{Email}'", employee.Email);
                    return Json(new { success = false, message = "An error occurred while saving. Please try again." });
                }
            }

            // Validation failed — re-populate dropdown and return partial with errors
            // jQuery re-injects this HTML into the modal so the user sees the errors
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_CreateEmployee", employee);
        }

        // ---------------------------------------------------------------
        // EDIT (GET) – Return partial view pre-filled with existing employee data
        // URL: GET /Employee/Edit/5  (called via AJAX)
        // ---------------------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null) return NotFound();

            // Populate department dropdown for the edit form
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_EditEmployee", employee);
        }

        // ---------------------------------------------------------------
        // EDIT (POST) – Save updated employee data
        // URL: POST /Employee/Edit  (submitted via AJAX)
        // Returns: JSON on success, partial view HTML on validation failure
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            // Duplicate email check — exclude the current employee from the check
            bool emailExists = await _context.Employees
                .AnyAsync(e => e.Email == employee.Email && e.EmployeeId != employee.EmployeeId);

            if (emailExists)
                ModelState.AddModelError("Email", "An employee with this email already exists.");

            if (ModelState.IsValid)
            {
                try
                {
                    // Update() marks all properties as modified — EF generates UPDATE SQL
                    _context.Employees.Update(employee);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Employee updated successfully." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating employee ID {Id}", employee.EmployeeId);
                    return Json(new { success = false, message = "An error occurred while saving. Please try again." });
                }
            }

            // Re-populate dropdown and return partial with validation errors
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_EditEmployee", employee);
        }

        // ---------------------------------------------------------------
        // DELETE (POST) – Permanently remove employee from database
        // URL: POST /Employee/Delete/5  (called via AJAX)
        // Returns: JSON with success/failure status
        // Note: Uses POST (not DELETE) because HTML forms only support GET and POST
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                    return Json(new { success = false, message = "Employee not found." });

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Employee deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee ID {Id}", id);
                return Json(new { success = false, message = "An error occurred while deleting. Please try again." });
            }
        }

        // ---------------------------------------------------------------
        // GET DEPARTMENT EMPLOYEE COUNT – AJAX endpoint
        // URL: GET /Employee/GetDepartmentEmployeeCount?departmentId=2
        // Returns: JSON { count: 5 }
        // Used by: department dropdown in Create/Edit form
        //          → shows "This department has X employees" to the user
        // ---------------------------------------------------------------
        public async Task<IActionResult> GetDepartmentEmployeeCount(int departmentId)
        {
            int count = await _context.Employees
                .CountAsync(e => e.DepartmentId == departmentId);

            return Json(new { count = count });
        }

        // ---------------------------------------------------------------
        // UPLOAD (GET) – Show the bulk upload page
        // URL: GET /Employee/Upload
        // ---------------------------------------------------------------
        public IActionResult Upload()
        {
            return View();
        }

        // ---------------------------------------------------------------
        // UPLOAD (POST) – Receive and process the uploaded CSV or Excel file
        // URL: POST /Employee/Upload
        // FileUploadService is injected directly into this action (not constructor)
        // because it's only needed for this one action
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file,
            [FromServices] FileUploadService uploadService)
        {
            // Validate that a file was actually selected
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return View();
            }

            // Validate file extension — only CSV and Excel are supported
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".csv" && ext != ".xlsx")
            {
                ModelState.AddModelError("", "Only .csv and .xlsx files are supported.");
                return View();
            }

            try
            {
                // Delegate all processing to FileUploadService
                // Returns a summary of how many rows succeeded/failed and why
                var result = await uploadService.ProcessFileAsync(file);
                return View("UploadResult", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing upload file '{FileName}'", file.FileName);
                ModelState.AddModelError("", "An error occurred while processing the file. Please check the format and try again.");
                return View();
            }
        }
    }
}