using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// Register ApplicationDbContext so EF knows how to connect to SQL Server.
// The connection string comes from appsettings.json → "DefaultConnection"
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();