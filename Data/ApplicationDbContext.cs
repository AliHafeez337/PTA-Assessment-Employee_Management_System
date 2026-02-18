using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    // ApplicationDbContext is the bridge between C# code and the SQL Server database.
    // Every time we want to read/write data, we go through this class.
    public class ApplicationDbContext : DbContext
    {
        // Constructor: receives options (like the connection string) from Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // These properties represent the database tables.
        // DbSet<Department> = the Departments table
        // DbSet<Employee>   = the Employees table
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        // OnModelCreating: fine-tune how EF maps our classes to the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Department configuration ---
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);                  // Primary key

                entity.Property(d => d.DepartmentName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(d => d.DepartmentCode)
                      .IsRequired()
                      .HasMaxLength(20);

                // Unique constraint: no two departments can share the same name
                entity.HasIndex(d => d.DepartmentName).IsUnique();
            });

            // --- Employee configuration ---
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);                    // Primary key

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                // Unique constraint: no two employees can share the same email
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Salary)
                      .HasColumnType("decimal(18,2)");

                // Relationship: one Department → many Employees
                // If a department is deleted, restrict (don't cascade delete employees)
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}