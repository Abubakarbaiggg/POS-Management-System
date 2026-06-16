using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using POS_Management_System.Models.ViewModels;
using System.Threading.Tasks;

namespace POS_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                ProductsCount = await _context.Products.CountAsync(),
                SuppliersCount = await _context.Suppliers.CountAsync(),
                CustomersCount = await _context.Customers.CountAsync(),
                SalesCount = await _context.Sales.CountAsync(),
                LowStockCount = await _context.Products.CountAsync(p => p.Quantity <= 10)
            };

            return View(model);
        }
    }
}
