using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // ApplicationDbContext is injected by ASP.NET's DI container —
        // gives us access to the database without creating it manually
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------------------------------------------------------------
        // INDEX – Dashboard page showing live statistics
        // URL: GET /  or  GET /Home
        // ---------------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel
            {
                // CountAsync() translates to: SELECT COUNT(*) FROM Employees
                TotalEmployees = await _context.Employees.CountAsync(),

                // Only count departments where ActiveInactive = 1
                TotalActiveDepartments = await _context.Departments
                    .CountAsync(d => d.ActiveInactive),

                // AverageAsync() throws an exception if the table is empty,
                // so we check AnyAsync() first — if no employees, default to 0
                AverageSalary = await _context.Employees.AnyAsync()
                    ? await _context.Employees.AverageAsync(e => e.Salary)
                    : 0
            };

            return View(vm);
        }
    }
}