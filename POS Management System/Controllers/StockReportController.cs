using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;

namespace POS_Management_System.Controllers
{
    public class StockReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StockReportController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, int? customerId, string? search)
        {
            var query = _context.StockOuts
                 .Include(x => x.Customer)
                 .Include(x => x.CustomerPayment)
                 .Include(x => x.StockOutDetails)
                   .ThenInclude(d => d.Product)
                 .AsQueryable();
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.Date.Date >= fromDate.Value.Date);
            }
            if (toDate.HasValue)
            {
                query = query.Where(x => x.Date.Date <= toDate.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Customer!.Name.Contains(search) ||
                    x.StockOutDetails.Any(d => d.Product!.Name.Contains(search)));
            }
            var data = await query.OrderByDescending(x => x.Date).ToListAsync();
            ViewBag.Customers = new SelectList(
                await _context.Customers.OrderBy(x => x.Name).ToListAsync(), "Id", "Name",
            customerId);

            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            ViewBag.Search = search;
            ViewBag.CustomerId = customerId;
            ViewBag.TotalInvoices = data.Count;
            ViewBag.TotalSales = data.Sum(x => x.CustomerPayment?.TotalAmount ?? 0);
            ViewBag.TotalPaid = data.Sum(x => x.CustomerPayment?.PaidAmount ?? 0);
            ViewBag.TotalRemaining = data.Sum(x => x.CustomerPayment?.RemainingAmount ?? 0);
            ViewBag.TotalItems = data.Sum(x => x.StockOutDetails.Sum(d => d.Quantity));

            return View(data);
        }
    }
}
