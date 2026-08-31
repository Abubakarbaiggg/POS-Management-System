using Hangfire;
using System;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Hubs;
using POS_Management_System.Models;
using POS_Management_System.Services;
using POS_Management_System.Services.Email;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");;

// Detect whether the connection string targets SQLite (file-based) or a SQL Server instance.
var useSqlite = connectionString.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase)
                || connectionString.EndsWith(".db", StringComparison.OrdinalIgnoreCase)
                || connectionString.Contains(".sqlite", StringComparison.OrdinalIgnoreCase);

if (useSqlite)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
}

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.AllowedForNewUsers = true;
     
     }).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
});
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
});
// Configure Hangfire only for SQL Server storage. When using SQLite this project will skip Hangfire SQL Server storage to avoid requiring the SQL Server-specific storage provider.
if (!useSqlite)
{
    builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
    builder.Services.AddHangfireServer();
}
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Ensure database directory exists when using SQLite so the file can be created inside container/host volume
        if (useSqlite)
        {
            try
            {
                // Attempt to get the data source path from the connection string
                var dataSource = connectionString.Split('=', 2)[1].Trim();
                var dbDir = Path.GetDirectoryName(dataSource);
                if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
                {
                    Directory.CreateDirectory(dbDir);
                }
            }
            catch
            {
                // ignore parsing failures; migration will still attempt to create the file if possible
            }
        }

        // Apply any pending EF Core migrations (works for SQLite and SQL Server)
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        await DbSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// Only enable Hangfire dashboard when SQL Server storage is in use.
if (!useSqlite)
{
    app.UseHangfireDashboard();
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<PosHub>("/posHub");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();


app.Run();
