namespace EmployeeManagementSystem.Models
{
    // Carries all dashboard statistics to the view
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }
        public int TotalActiveDepartments { get; set; }
        public decimal AverageSalary { get; set; }
    }
}