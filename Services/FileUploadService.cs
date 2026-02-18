using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services
{
    public class FileUploadService
    {
        private readonly ApplicationDbContext _context;

        public FileUploadService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ─── MAIN ENTRY POINT ─────────────────────────────────────
        // Reads the file, processes each row, returns summary
        public async Task<UploadResult> ProcessFileAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();

            // Read rows from file depending on extension
            List<string[]> rows = ext == ".csv"
                ? ReadCsv(file)
                : ReadExcel(file);

            return await ProcessRowsAsync(rows);
        }

        // ─── CSV READER ───────────────────────────────────────────
        private List<string[]> ReadCsv(IFormFile file)
        {
            var rows = new List<string[]>();

            using var reader = new StreamReader(file.OpenReadStream());

            // Skip header row
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                rows.Add(line.Split(','));
            }

            return rows;
        }

        // ─── EXCEL READER ─────────────────────────────────────────
        private List<string[]> ReadExcel(IFormFile file)
        {
            var rows = new List<string[]>();

            using var stream = file.OpenReadStream();
            using var package = new ExcelPackage(stream);

            var sheet = package.Workbook.Worksheets[0];
            int totalRows = sheet.Dimension?.Rows ?? 0;

            // Start from row 2 — row 1 is the header
            for (int r = 2; r <= totalRows; r++)
            {
                rows.Add(new[]
                {
                    sheet.Cells[r, 1].Text,  // Name
                    sheet.Cells[r, 2].Text,  // Email
                    sheet.Cells[r, 3].Text,  // Salary
                    sheet.Cells[r, 4].Text,  // Department Name
                    sheet.Cells[r, 5].Text   // Joining Date
                });
            }

            return rows;
        }

        // ─── ROW PROCESSOR ────────────────────────────────────────
        private async Task<UploadResult> ProcessRowsAsync(List<string[]> rows)
        {
            var result = new UploadResult { TotalRows = rows.Count };

            // Cache existing emails to avoid N+1 DB calls
            var existingEmails = await _context.Employees
                .Select(e => e.Email.ToLower())
                .ToListAsync();

            // Cache department names (case-insensitive lookup)
            var departments = await _context.Departments.ToListAsync();

            // Track emails seen in this file to catch duplicates within the upload
            var emailsInThisFile = new HashSet<string>();

            for (int i = 0; i < rows.Count; i++)
            {
                int rowNum = i + 2; // +2 because row 1 is the header
                var cols = rows[i];

                var rowResult = new RowResult { RowNumber = rowNum };

                // ── Extract columns ───────────────────────────────
                string name         = cols.Length > 0 ? cols[0].Trim() : "";
                string email        = cols.Length > 1 ? cols[1].Trim() : "";
                string salaryStr    = cols.Length > 2 ? cols[2].Trim() : "";
                string deptName     = cols.Length > 3 ? cols[3].Trim() : "";
                string dateStr      = cols.Length > 4 ? cols[4].Trim() : "";

                rowResult.EmployeeName = name;

                // ── Validate ──────────────────────────────────────
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(name))
                    errors.Add("Name is required");

                if (string.IsNullOrWhiteSpace(email))
                    errors.Add("Email is required");
                else if (!email.Contains("@"))
                    errors.Add("Invalid email format");
                else if (existingEmails.Contains(email.ToLower()))
                    errors.Add("Email already exists in database");
                else if (emailsInThisFile.Contains(email.ToLower()))
                    errors.Add("Duplicate email in file");

                if (!decimal.TryParse(salaryStr, out decimal salary) || salary <= 0)
                    errors.Add("Salary must be a positive number");

                if (string.IsNullOrWhiteSpace(deptName))
                    errors.Add("Department name is required");

                if (!DateTime.TryParse(dateStr, out DateTime joiningDate))
                    errors.Add("Invalid joining date format");

                // ── If errors — record and skip ───────────────────
                if (errors.Count > 0)
                {
                    rowResult.IsSuccess = false;
                    rowResult.ErrorMessage = string.Join("; ", errors);
                    result.Rows.Add(rowResult);
                    result.FailedCount++;
                    continue;
                }

                // ── Find or create department ─────────────────────
                var dept = departments.FirstOrDefault(d =>
                    d.DepartmentName.Equals(deptName, StringComparison.OrdinalIgnoreCase));

                if (dept == null)
                {
                    // Auto-create department
                    dept = new Department
                    {
                        DepartmentName = deptName,
                        DepartmentCode = deptName.Length >= 3
                            ? deptName.Substring(0, 3).ToUpper()
                            : deptName.ToUpper(),
                        ActiveInactive = true,
                        CreatedDate = DateTime.Now
                    };
                    _context.Departments.Add(dept);
                    await _context.SaveChangesAsync();  // Save to get the new DepartmentId
                    departments.Add(dept);              // Add to cache so next row finds it
                }

                // ── Insert employee ───────────────────────────────
                var employee = new Employee
                {
                    Name = name,
                    Email = email,
                    Salary = salary,
                    DepartmentId = dept.DepartmentId,
                    JoiningDate = joiningDate
                };

                _context.Employees.Add(employee);
                existingEmails.Add(email.ToLower());   // Update cache
                emailsInThisFile.Add(email.ToLower()); // Track in-file duplicates

                rowResult.IsSuccess = true;
                result.Rows.Add(rowResult);
                result.SuccessCount++;
            }

            // Save all valid employees in one batch
            await _context.SaveChangesAsync();

            return result;
        }
    }
}