using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel
            {
                TotalEmployees = await _context.Employees.CountAsync(),

                TotalActiveDepartments = await _context.Departments
                    .CountAsync(d => d.ActiveInactive),

                // AverageAsync throws if the table is empty — check first
                AverageSalary = await _context.Employees.AnyAsync()
                    ? await _context.Employees.AverageAsync(e => e.Salary)
                    : 0
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}