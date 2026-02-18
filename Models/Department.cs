using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Department
    {
        // Primary key - auto-incremented by SQL Server
        public int DepartmentId { get; set; }

        // Department name - required, max 100 characters
        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; } = string.Empty;

        // Short code like "IT", "HR", "FIN"
        [Required(ErrorMessage = "Department code is required")]
        [StringLength(20, ErrorMessage = "Code cannot exceed 20 characters")]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; } = string.Empty;

        // true = Active, false = Inactive (soft delete uses this)
        [Display(Name = "Active")]
        public bool ActiveInactive { get; set; } = true;

        // Automatically set when department is created
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property: one department has many employees
        // "virtual" allows Entity Framework lazy loading
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}