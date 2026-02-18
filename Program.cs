using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Services;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// ── SERVICES ──────────────────────────────────────────────────────────
// Services are registered here before app.Build() is called.
// ASP.NET's DI container reads these registrations and injects
// the correct instances into controllers automatically.

// Register MVC — enables Controllers and Razor Views
builder.Services.AddControllersWithViews();

// Register Entity Framework with SQL Server.
// The connection string is read from appsettings.json → "DefaultConnection"
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// EPPlus v5+ requires a license context to be set before use.
// NonCommercial is correct for personal/educational projects.
ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

// Register FileUploadService as Scoped — one instance per HTTP request.
// This is appropriate because it depends on ApplicationDbContext,
// which is also scoped to the request lifetime.
builder.Services.AddScoped<FileUploadService>();

// ── BUILD ──────────────────────────────────────────────────────────────
var app = builder.Build();

// ── MIDDLEWARE PIPELINE ────────────────────────────────────────────────
// Middleware runs in the order it is added here.

if (!app.Environment.IsDevelopment())
{
    // In production: redirect unhandled exceptions to the custom error page
    app.UseExceptionHandler("/Home/Error");
    // Enable HTTP Strict Transport Security (HTTPS-only header)
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable routing so the framework can match URLs to controller actions
app.UseRouting();

// Enable authorization middleware (required even if no auth is configured)
app.UseAuthorization();

// Serve static files from wwwroot/ (CSS, JS, Bootstrap, jQuery, images)
app.MapStaticAssets();

// Define the default route pattern:
// /{controller}/{action}/{id?}
// Defaults: controller=Home, action=Index
// Example: "/" → HomeController.Index()
//          "/Employee" → EmployeeController.Index()
//          "/Department/Edit/3" → DepartmentController.Edit(3)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();