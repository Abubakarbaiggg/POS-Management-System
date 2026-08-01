# POS Management System

Short guide and setup notes for the POS Management System project.

Overview
- Project: POS Management System
- Type: ASP.NET Core MVC (Razor Views) with Identity
- Target framework: .NET 10

Quick start
1. Update connection string in appsettings.json (DefaultConnection).
2. Configure EmailSettings in appsettings.json or secrets.
3. In a development environment run migrations and update database:
   - dotnet ef database update
4. Build and run from Visual Studio or dotnet run.

Configuration
- appsettings.json contains database and email configuration.

Important files
- Program.cs — application startup, DI, Identity, Hangfire.
- Data/ApplicationDbContext.cs — EF Core DbContext and DbSets.
- Controllers/ — MVC controllers for modules (Products, StockIn/Out, Suppliers, Customers, Users, Roles, Permissions, Reports, Dashboard).
- Models/ — entity classes and view models.
- Views/ — Razor views and shared layout.

Where to find documentation
- Detailed docs: PROJECT_DOCUMENTATION.md
- Module summary: MODULES.md
- Feature list: FEATURES.md
