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
        private readonly Microsoft.Extensions.Logging.ILogger<StockOutController> _logger;

        public StockOutController(ApplicationDbContext context, Hangfire.IBackgroundJobClient backgroundJobs, Microsoft.Extensions.Logging.ILogger<StockOutController> logger)
        {
            _context = context;
            _backgroundJobs = backgroundJobs;
            _logger = logger;
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
            // Log incoming form and productsJson for diagnostics
            try
            {
                _logger?.LogInformation("StockOut Create POST called. productsJson present: {HasProductsJson}", !string.IsNullOrEmpty(productsJson));
                foreach (var item in Request.Form)
                {
                    _logger?.LogDebug("Form field: {Key} = {Value}", item.Key, item.Value.ToString());
                }
                _logger?.LogDebug("Raw productsJson: {ProductsJson}", productsJson);
            }
            catch { }

            if (string.IsNullOrEmpty(productsJson))
            {
                ModelState.AddModelError("", "Please add at least one product");
                await LoadViewBags(model);
                _logger?.LogWarning("Create aborted: productsJson empty or null. ModelState: {@ModelState}", ModelState);
                return View(model);
            }

            List<StockOutProductItem>? products = null;
            try
            {
                products = JsonSerializer.Deserialize<List<StockOutProductItem>>(productsJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _logger?.LogInformation("Deserialized products count: {Count}", products == null ? 0 : products.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to deserialize productsJson: {ProductsJson}", productsJson);
                ModelState.AddModelError("", "Invalid products data");
                await LoadViewBags(model);
                return View(model);
            }

            if (products == null || products.Count == 0)
            {
                ModelState.AddModelError("", "Please add at least one product");
                await LoadViewBags(model);
                return View(model);
            }

            foreach (var item in products)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("", $"Product with ID {item.ProductId} does not exist in database");
                    await LoadViewBags(model);
                    return View(model);
                }

                if (item.Quantity <= 0)
                {
                    ModelState.AddModelError("", $"Quantity must be greater than 0 for {item.ProductName}");
                    await LoadViewBags(model);
                    return View(model);
                }

                if (product.Quantity < item.Quantity)
                {
                    ModelState.AddModelError("", $"Not enough stock for {item.ProductName}. Available: {product.Quantity}");
                    await LoadViewBags(model);
                    return View(model);
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadViewBags(model);
                return View(model);
            }

            decimal totalAmount = products.Sum(p => p.Total);
         

            var stockOut = new StockOut
            {
                CustomerId = model.CustomerId,
                Date = model.Date
            };

            _context.StockOuts.Add(stockOut);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to save StockOut entity to database. StockOut: {@StockOut}", stockOut);
                ModelState.AddModelError("", "An error occurred while saving the stock out. Please try again.");
                await LoadViewBags(model);
                return View(model);
            }

            var payment = new Payment
            {
                StockOutId = stockOut.Id,
                TotalAmount = totalAmount,
                PaidAmount = model.PaidAmount,
                RemainingAmount = totalAmount - model.PaidAmount,
                PaymentMethod = model.PaymentMethod
            };

            //_context.Payments.Add(payment);

            foreach (var item in products)
            {
                var stockOutDetail = new StockOutDetail
                {
                    StockOutId = stockOut.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Total = item.Total
                };
                _context.StockOutDetails.Add(stockOutDetail);

                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;

                    if (product.Quantity <= 5)
                    {
                        try
                        {
                            var jobId = _backgroundJobs.Enqueue<IEmailService>(
                                x => x.SendLowStockAlertAsync(
                                    product.Name,
                                    product.Quantity
                                ));
                            _logger?.LogInformation("Enqueued low stock email job {JobId} for product {Product} (stock {Stock})", jobId, product.Name, product.Quantity);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "Failed to enqueue Hangfire job for low stock alert for product {Product}", product.Name);
                        }
                    }
                    _context.Update(product);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to save StockOut details/products to database. StockOutId: {Id}", stockOut.Id);
                ModelState.AddModelError("", "An error occurred while saving the stock out details. Please try again.");
                await LoadViewBags(model);
                return View(model);
            }

            TempData["Success"] = $"Stock Out completed successfully! Total amount: {totalAmount:N2}";
            return RedirectToAction(nameof(Index));
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
