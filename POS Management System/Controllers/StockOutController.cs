using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using POS_Management_System.Models.ViewModels;
using POS_Management_System.Services.Email;
using System.Text.Json;

namespace POS_Management_System.Controllers
{
    [Authorize]

    public class StockOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Hangfire.IBackgroundJobClient _backgroundJobs;
        public StockOutController(ApplicationDbContext context, Hangfire.IBackgroundJobClient backgroundJobs)
        {
            _context = context;
            _backgroundJobs = backgroundJobs;
        }

        public async Task<IActionResult> Index()
        {
            var stockOuts = await _context.StockOuts
                .Include(s => s.Customer)
                .Include(s => s.StockOutDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
            return View(stockOuts);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name");
            ViewBag.Products = await _context.Products.ToListAsync();

            return View(new StockOutViewModel
            {
                Date = DateTime.Now,
                Products = new List<StockOutProductItem>()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockOutViewModel model, string productsJson)
        {
           
            var products = GetProducts(productsJson);

            if (products == null || !products.Any())
            {
                return await ReturnWithError(model, "Please add at least one product");
            }

            var validationError = await ValidateProducts(products);
            
            if (validationError != null)
            {
                return await ReturnWithError(model, validationError);
            }

            decimal totalAmount = products.Sum(p => p.Total);
         

            var stockOut = new StockOut
            {
                CustomerId = model.CustomerId,
                Date = model.Date
            };

            _context.StockOuts.Add(stockOut);
         
            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                StockOutId = stockOut.Id,
                TotalAmount = totalAmount,
                PaidAmount = model.PaidAmount,
                RemainingAmount = totalAmount - model.PaidAmount,
                PaymentMethod = model.PaymentMethod
            };


            _context.Payments.Add(payment);

            await SaveStockOutDetails(stockOut.Id, products);

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Stock Out completed successfully! Total amount: {totalAmount:N2}";
            return RedirectToAction(nameof(Index));
        }
        private List<StockOutProductItem>? GetProducts(string productsJson)
        {
            if (string.IsNullOrWhiteSpace(productsJson))
                return null;
            return JsonSerializer.Deserialize<List<StockOutProductItem>>(
                    productsJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
        }
        private async Task<string?> ValidateProducts(List<StockOutProductItem> products)
        {
            foreach (var item in products)
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null)
                    return $"Product with ID {item.ProductId} does not exist.";

                if (item.Quantity <= 0)
                    return $"Quantity must be greater than 0 for {item.ProductName}.";

                if (product.Quantity < item.Quantity)
                    return $"Not enough stock for {item.ProductName}. Available: {product.Quantity}.";
            }

            return null;
        }
        private async Task<IActionResult> ReturnWithError(StockOutViewModel model,string message)
        {
            ModelState.AddModelError("", message);
            await LoadViewBags(model);
            return View(model);
        }
        private async Task SaveStockOutDetails(int stockOutId, List<StockOutProductItem> products)
        {
            foreach (var item in products)
            {
                _context.StockOutDetails.Add(new StockOutDetail
                {
                    StockOutId = stockOutId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Total = item.Total
                });

                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null)
                    continue;

                product.Quantity -= item.Quantity;

                if (product.Quantity <= 5)
                {
                    _backgroundJobs.Enqueue<IEmailService>(x =>
                        x.SendLowStockAlertAsync(product.Name, product.Quantity));
                }

                _context.Products.Update(product);
            }
        }
        private async Task LoadViewBags(StockOutViewModel model)
        {
            ViewBag.Customers = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name", model.CustomerId);
            ViewBag.Products = await _context.Products.ToListAsync();
        }

        public async Task<IActionResult> Details(int id)
        {
            var stockOut = await _context.StockOuts
                .Include(s => s.Customer)
                .Include(s => s.StockOutDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (stockOut == null)
            {
                return NotFound();
            }

            return View(stockOut);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var stockOut = await _context.StockOuts
                .Include(s => s.Customer)
                .Include(s => s.StockOutDetails)
                 .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (stockOut == null)
            {
                return NotFound();
            }

            return View(stockOut);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockOut = await _context.StockOuts
                .Include(s => s.StockOutDetails)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stockOut != null)
            {
                foreach (var detail in stockOut.StockOutDetails!)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product != null)
                    {
                        product.Quantity += detail.Quantity;
                        _context.Update(product);
                    }
                }

                _context.StockOuts.Remove(stockOut);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Stock Out record deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
