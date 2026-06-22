using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using POS_Management_System.ViewModels;
using System.Threading.Tasks;

namespace POS_Management_System.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var model = new DashboardViewModel
        //    {
        //        ProductsCount = await _context.Products.CountAsync(),
        //        SuppliersCount = await _context.Suppliers.CountAsync(),
        //        CustomersCount = await _context.Customers.CountAsync(),
        //        SalesCount = await _context.Sales.CountAsync(),
        //        LowStockCount = await _context.Products.CountAsync(p => p.Quantity <= 10)
        //    };

        //    return View(model);
        //}
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var model = new DashboardViewModel
            {
                ProductsCount = await _context.Products.CountAsync(),
                SuppliersCount = await _context.Suppliers.CountAsync(),
                CustomersCount = await _context.Customers.CountAsync(),

                LowStockCount = await _context.Products.CountAsync(p => p.Quantity < 5),

                TodaySales = await _context.StockOutDetails
                    .Where(x => x.StockOut.Date.Date == today)
                    .SumAsync(x => (decimal?)x.Total) ?? 0,

                MonthlySales = await _context.StockOutDetails
                    .Where(x => x.StockOut.Date >= startOfMonth)
                    .SumAsync(x => (decimal?)x.Total) ?? 0,

                MonthlyStockIn = await _context.StockInDetails
                    .Where(x => x.StockIn.Date >= startOfMonth)
                    .SumAsync(x => (decimal?)x.Total) ?? 0,

                TotalStockIn = await _context.StockInDetails.SumAsync(x => (decimal?)x.Total) ?? 0,
                TotalStockOut = await _context.StockOutDetails.SumAsync(x => (decimal?)x.Total) ?? 0
            };

            return View(model);
        }
    }
}
