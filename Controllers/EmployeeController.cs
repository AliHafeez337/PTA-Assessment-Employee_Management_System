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

        public EmployeeController(ApplicationDbContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Employee/Index
        public async Task<IActionResult> Index(string? searchName, int? departmentId)
        {
            try
            {
                var query = _context.Employees
                    .Include(e => e.Department)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchName))
                    query = query.Where(e => e.Name.Contains(searchName));

                if (departmentId.HasValue && departmentId.Value > 0)
                    query = query.Where(e => e.DepartmentId == departmentId.Value);

                var employees = await query.OrderBy(e => e.Name).ToListAsync();

                ViewBag.Departments = await _context.Departments
                    .Where(d => d.ActiveInactive)
                    .OrderBy(d => d.DepartmentName)
                    .ToListAsync();

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

        // GET: /Employee/Create
        // Returns partial view (popup form) for adding an employee
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_CreateEmployee", new Employee());
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            // Duplicate email check
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

                    return Json(new { success = true, message = "Employee added successfully." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating employee '{Email}'", employee.Email);
                    return Json(new { success = false, message = "An error occurred while saving. Please try again." });
                }
            }

            // Validation failed — re-populate dropdown and return partial with errors
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ActiveInactive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return PartialView("_CreateEmployee", employee);
        }

        // GET: /Employee/Edit/5
        // Returns partial view pre-filled with employee data
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null) return NotFound();

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
            // Duplicate email check — exclude current employee
            bool emailExists = await _context.Employees
                .AnyAsync(e => e.Email == employee.Email && e.EmployeeId != employee.EmployeeId);

            if (emailExists)
                ModelState.AddModelError("Email", "An employee with this email already exists.");

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: /Employee/GetDepartmentEmployeeCount/5
        // AJAX endpoint — returns employee count for a department
        public async Task<IActionResult> GetDepartmentEmployeeCount(int departmentId)
        {
            int count = await _context.Employees
                .CountAsync(e => e.DepartmentId == departmentId);

            return Json(new { count = count });
        }

        // GET: /Employee/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: /Employee/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file,
            [FromServices] FileUploadService uploadService)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return View();
            }

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".csv" && ext != ".xlsx")
            {
                ModelState.AddModelError("", "Only .csv and .xlsx files are supported.");
                return View();
            }

            try
            {
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