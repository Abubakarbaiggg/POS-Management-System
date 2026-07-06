using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using System.Text.Json;
using System.Text.Json.Serialization;


public class SaleReportController : Controller
{
    private readonly ApplicationDbContext _context;

    public SaleReportController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new List<StockOut>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
    {
        var query = _context.StockOuts
                .Include(x => x.Customer)
                .Include(x => x.CustomerPayment)
                .AsQueryable();


        if (fromDate.HasValue)
        {
            query = query.Where(x => x.Date >= fromDate.Value.Date);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.Date <= toDate.Value.Date);
        }

        var sales = await query.OrderByDescending(x => x.Date).ToListAsync();

        Console.WriteLine(JsonSerializer.Serialize(sales, new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        }));

        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

        return View(sales);
    }
}