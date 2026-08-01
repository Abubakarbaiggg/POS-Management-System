# POS Management System - Project Documentation

Generated overview and module-level documentation for the POS Management System.

1. Project Overview
- Project Name: POS Management System
- Purpose: Basic point-of-sale management including products, stock in/out, suppliers, customers, sales, and reports.
- Technology Stack: ASP.NET Core MVC (Razor Views), Entity Framework Core, SQL Server, Hangfire, ASP.NET Core Identity
- Framework Version: .NET 10
- Database: SQL Server (connection string in appsettings.json)
- Architecture Pattern: Layered MVC with Repository-like patterns implemented via DbContext usage, services for email and background jobs.

2. Project Modules
The project includes the following modules. Each module is implemented with controllers, views, and models under the Controllers, Views, and Models folders.

## Product Module
- Description: Manage products with images, pricing, and stock quantity.
- Purpose: CRUD operations for products and stock management.
- Features: Create, Edit, Delete, List, Update Stock, Image upload.
- CRUD Operations: ProductController (Index, Create, Edit, Delete, Stock)
- Validation Rules: Name required (<=200), SalePrice & PurchasePrice required, Quantity required.
- User Permissions: Requires authenticated user. Role-based permissions not enforced at controller level in code, project includes role management controllers.
- Dependencies: Category, Product image files, IIS/static assets.
- Related Database Tables: Products, Categories

## Category Module
- Description: Product categories.
- Purpose: Organize products by categories.
- Features: Create, Edit, Delete, List
- CRUD Operations: CategoryController
- Validation Rules: Name required (<=100)
- User Permissions: Authenticated users required
- Dependencies: Products
- Related Database Tables: Categories

## Stock In Module
- Description: Record stock procurement from suppliers.
- Purpose: Inbound stock with supplier and payment information.
- Features: Create stock in, list, details, delete, validation for product quantities
- CRUD Operations: StockInController
- Validation Rules: Products list required, each quantity > 0
- User Permissions: Authenticated users
- Dependencies: Suppliers, Products, SupplierPayments, StockInDetails
- Related Database Tables: StockIns, StockInDetails, SupplierPayments

## Stock Out / Sales Module
- Description: Record stock sold to customers with payments
- Purpose: Outbound stock, sales recording, low-stock alerts
- Features: Create stock out, validation against product quantity, background low stock email via Hangfire
- CRUD Operations: StockOutController
- Validation Rules: Quantities > 0 and <= available stock
- User Permissions: Authenticated users
- Dependencies: Customers, Products, CustomerPayments, StockOutDetails
- Related Database Tables: StockOuts, StockOutDetails, CustomerPayments

## Supplier Module
- Description: Manage suppliers
- Purpose: CRUD for supplier entities
- Features: Create/Edit/Delete/List
- CRUD Operations: SupplierController
- Validation Rules: Required name, phone, optional email
- Related Database Tables: Suppliers

## Customer Module
- Description: Manage customers
- Purpose: CRUD for customer entities
- Features: Create/Edit/Delete/List
- CRUD Operations: CustomerController
- Validation Rules: Required name, phone, optional email
- Related Database Tables: Customers

## Users, Roles & Permissions
- Description: Identity-based user management with role assignment and a Permission model
- Purpose: Manage application users, roles and map permissions
- Features: User create/edit/delete, assign roles, roles management, permissions pages
- CRUD Operations: UsersController, RolesController, PermissionsController
- Related Database Tables: AspNetUsers, AspNetRoles, Permission (custom)

## Reports
- SaleReportController: Sales listing and Excel export (ClosedXML)
- CustomerReportController, SupplierReportController, ProfitReportController: controllers exist for reporting (views present)

3. Authentication & Authorization
- Uses ASP.NET Core Identity (AddDefaultIdentity and AddRoles in Program.cs)
- Login and registration implemented in Areas/Identity
- Cookie settings: custom login path and access denied path in Program.cs
- Role management: RolesController and usage of RoleManager and UserManager in UsersController

4. Database Documentation
- DbContext: ApplicationDbContext with DbSets for Products, Suppliers, StockIns, StockInDetails, StockOuts, StockOutDetails, Sales, SaleDetails, Customers, Categories, Permissions, CustomerPayments, SupplierPayments
- Migrations: Present under Migrations folder (several migrations for schema changes, permissions, payments tables)

5. Controller Documentation
- ProductController: Index (GET), Create (GET/POST), Edit (GET/POST), Delete (GET/POST), Stock (GET/POST)
- CategoryController: Index, Create, Edit, Delete
- StockInController: Index, Create (GET/POST), Details, Delete
- StockOutController: Index, Create (GET/POST), Details, Delete
- SupplierController: Index, Create, Edit, Delete
- CustomerController: Index, Create, Edit, Delete
- UsersController: Index, Create, Edit, Details, Delete, AssignRole
- SaleReportController: Index (GET/POST), Download (GET) - Excel export using ClosedXML

6. Model Documentation
- Product: Id(int), CategoryId(int), Name(string, required), SalePrice(decimal), PurchasePrice(decimal), Quantity(int), ImagePath(string), Category(nav), ImageFile(not mapped)
- Category: Id(int), Name(string required)
- StockIn/Out and Details: typical master-detail with totals and purchase/sale prices
- Supplier, Customer: contact info, email validation
- Permission: basic permission model

7. Business Logic
- EmailService sends low-stock alerts; integrated with Hangfire to send background email jobs when quantity low on stock out
- PaginatedList helper for pagination in StockIn/Out

8. User Flow
- Login -> Dashboard -> Manage Products/Categories -> Stock In -> Stock Out/Sales -> Reports -> Logout

9. Reports
- Sales report with date filters and Excel export
- Other report controllers present (Customer/Supplier/Profit/Stock)

10. Dashboard
- DashboardViewModel exposes counts and totals: ProductsCount, SuppliersCount, CustomersCount, LowStockCount, TodaySales, MonthlySales, MonthlyStockIn, TotalStockIn, TotalStockOut

11. Security
- Identity with role support
- Cookies configured with login and access denied paths
- CSRF protection enabled via ValidateAntiForgeryToken on POST actions

12. Project Features
- Full CRUD for products, categories, suppliers, customers
- Stock in & out with inventory updates
- User management with roles and permissions
- Reports and Excel export
- Low stock email alerts via Hangfire

13. Missing Features (suggested)
- API endpoints for external POS terminals
- Unit and integration tests
- Audit logging for critical operations
- Role-based authorization enforcement per action

14. Future Enhancements
- Multi-tenant support
- Background reconciliation jobs
- Real-time dashboards via SignalR
- External payment gateways integration

15. Folder Structure
- Controllers/ - MVC controllers
- Models/ - Domain models and view models
- Views/ - Razor views
- Data/ - ApplicationDbContext and migrations
- Services/ - email service
- wwwroot/ - static assets (css/js/images)

16. Dependencies
- Microsoft.AspNetCore.Identity.EntityFrameworkCore - identity
- Microsoft.EntityFrameworkCore.SqlServer - EF Core SQL Server provider
- Hangfire - background jobs
- ClosedXML - Excel export
- jQuery, Bootstrap - UI

17. APIs
- No external HTTP API endpoints beyond MVC controllers; closed system for now.

18. Screens
- Dashboard, Products Index/Create/Edit/Delete/Stock, Categories Index/Create/Edit/Delete, Suppliers Index/Create/Edit/Delete, Customers Index/Create/Edit/Delete, StockIn Index/Create/Details/Delete, StockOut Index/Create/Details/Delete, Reports pages, Users management pages, Roles and Permissions pages

19. Database Diagram
- ERD: Products -> Category (CategoryId FK), StockIn -> Supplier (SupplierId FK), StockInDetail -> StockIn, Product, StockOut -> Customer, StockOutDetail -> StockOut, Product, Sales similar mapping.

20. Module Summary
| Module | Description | CRUD | Permission | Status |
|--------|-------------|------|------------|--------|
| Product | Manage products & stock | Yes | Authenticated | Implemented |
| Category | Manage categories | Yes | Authenticated | Implemented |
| StockIn | Stock procurement | Yes | Authenticated | Implemented |
| StockOut | Sales / stock out | Yes | Authenticated | Implemented |
| Supplier | Manage suppliers | Yes | Authenticated | Implemented |
| Customer | Manage customers | Yes | Authenticated | Implemented |
| Users/Roles | Identity management | Yes | Admin roles available | Implemented |

Files generated:
- README.md
- PROJECT_DOCUMENTATION.md (this file)
- ChatGpt-Module-README.txt (previously added)

Notes:
- This documentation was generated by scanning project files in the repository. It is recommended to review and enrich descriptions, add request/response samples for any API endpoints you will expose, and update permission rules if you will enforce role-based checks.
