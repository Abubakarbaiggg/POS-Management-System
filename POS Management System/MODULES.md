# Modules

Detailed module list for the POS Management System.

## Product Module
- Location: Controllers/ProductController.cs, Models/Product.cs, Views/Product
- Features: Create, Edit, Delete, List, Stock update, Image upload

## Category Module
- Location: Controllers/CategoryController.cs, Models/Category.cs, Views/Category
- Features: Create, Edit, Delete, List

## Stock In Module
- Location: Controllers/StockInController.cs, Models/StockIn.cs, Models/StockInDetail.cs, Views/StockIn
- Features: Add stock, list, details, remove

## Stock Out Module
- Location: Controllers/StockOutController.cs, Models/StockOut.cs, Models/StockOutDetail.cs, Views/StockOut
- Features: Add sale/stock out, validation against inventory, low stock email alerts

## Supplier Module
- Location: Controllers/SupplierController.cs, Models/Supplier.cs, Views/Supplier
- Features: CRUD

## Customer Module
- Location: Controllers/CustomerController.cs, Models/Customer.cs, Views/Customer
- Features: CRUD

## Users / Roles / Permissions
- Location: Controllers/UsersController.cs, Controllers/RolesController.cs, Controllers/PermissionsController.cs, Areas/Identity
- Features: User management, roles assignment, permissions listing

## Reports
- Location: Controllers/SaleReportController.cs, Controllers/CustomerReportController.cs, Controllers/SupplierReportController.cs, Controllers/ProfitReportController.cs
- Features: Date-filtered reports, Excel export (ClosedXML)
