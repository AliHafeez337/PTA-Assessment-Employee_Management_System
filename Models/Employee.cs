using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        // Primary key - auto-incremented by SQL Server
        public int EmployeeId { get; set; }

        // Employee full name
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        // Email must be unique - enforced at DB level via EF config
        [Required(ErrorMessage = "Email is required")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        // Salary: positive decimal value
        [Required(ErrorMessage = "Salary is required")]
        [Column(TypeName = "decimal(18,2)")]     // 18 digits total, 2 decimal places
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0")]
        public decimal Salary { get; set; }

        // Foreign key linking to Departments table
        [Required(ErrorMessage = "Please select a department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        // Date the employee joined the company
        [Required(ErrorMessage = "Joining date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        // Navigation property: reference to the parent Department object
        // [ForeignKey] tells EF which property is the FK
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }
    }
}