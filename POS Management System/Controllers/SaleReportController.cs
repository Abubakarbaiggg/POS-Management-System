using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClosedXML.Excel;


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

    [HttpGet]
    public async Task<IActionResult> Download(DateTime? fromDate, DateTime? toDate)
    {
        var query = _context.StockOuts
            .Include(x => x.Customer)
            .Include(x => x.CustomerPayment)
            .AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(x => x.Date >= fromDate.Value.Date);

        if (toDate.HasValue)
            query = query.Where(x => x.Date <= toDate.Value.Date);

        var sales = await query.OrderByDescending(x => x.Date).ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sales");

        worksheet.Cell(1, 1).Value = "#";
        worksheet.Cell(1, 2).Value = "Customer";
        worksheet.Cell(1, 3).Value = "Date";
        worksheet.Cell(1, 4).Value = "Total";
        worksheet.Cell(1, 5).Value = "Paid";
        worksheet.Cell(1, 6).Value = "Remaining";
        worksheet.Cell(1, 7).Value = "Payment Method";

        int row = 2;
        int sr = 1;


        foreach (var sale in sales)
        {
            worksheet.Cell(row, 1).Value = sr++;
            worksheet.Cell(row, 2).Value = sale.Customer?.Name ?? "Walk In Customer";
            worksheet.Cell(row, 3).Value = sale.Date.ToString("dd MMM yyyy");
            worksheet.Cell(row, 4).Value = sale.CustomerPayment?.TotalAmount ?? 0;
            worksheet.Cell(row, 5).Value = sale.CustomerPayment?.PaidAmount ?? 0;
            worksheet.Cell(row, 6).Value = sale.CustomerPayment?.RemainingAmount ?? 0;
            worksheet.Cell(row, 7).Value = sale.CustomerPayment != null ? sale.CustomerPayment.PaymentMethod.ToString() : "";

            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "SalesReport.xlsx");
    }
}