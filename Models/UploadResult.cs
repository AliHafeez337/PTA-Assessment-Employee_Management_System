namespace EmployeeManagementSystem.Models
{
    // Carries the result of a single uploaded row
    public class RowResult
    {
        public int RowNumber { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
    }

    // Carries the full upload summary shown to the user
    public class UploadResult
    {
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<RowResult> Rows { get; set; } = new List<RowResult>();
    }
}