using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using POS_Management_System.Data;
using POS_Management_System.Helpers;
using POS_Management_System.Models;
using POS_Management_System.Models.ViewModels;
using POS_Management_System.Services.Email;
using System.Text.Json;

namespace POS_Management_System.Controllers
{
    [Authorize]

    public class StockInController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockInController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            int pageSize = 5;
            var stockIns = await PaginatedList<StockIn>.CreateAsync(
                _context.StockIns
                .Include(s => s.Supplier)
                .Include(s => s.SupplierPayment)
                .Include(s => s.StockInDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(s => s.Date),
                page,pageSize);
            return View(stockIns);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "Id", "Name");
            ViewBag.Products = await _context.Products.ToListAsync();

            return View(new StockInViewModel
            {
                Date = DateTime.Now,
                Products = new List<StockInProductItem>()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockInViewModel model, string productsJson)
        {
            var products = GetProducts(productsJson);

            if (products == null || products.Count == 0)
            {
                ModelState.AddModelError("", "Please add at least one product");
                await LoadViewBags(model);
                return View(model);
            }

            var validationError = await ValidateProducts(products);
            if (validationError != null)
            {
                return await ReturnWithError(model, validationError);
            }

            decimal totalAmount = products.Sum(p => p.Total);

            var stockIn = new StockIn
            {
                SupplierId = model.SupplierId,
                Date = model.Date
            };

            _context.StockIns.Add(stockIn);
            await _context.SaveChangesAsync();

            var payment = new SupplierPayment
            {
                StockInId = stockIn.Id,
                TotalAmount = totalAmount,
                PaidAmount = model.PaidAmount,
                RemainingAmount = totalAmount - model.PaidAmount,
                PaymentMethod = model.PaymentMethod
            };
            _context.SupplierPayments.Add(payment);

            await SaveStockInDetails(stockIn.Id, products);

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Stock In completed successfully! Total amount: {totalAmount:N2}";
            return RedirectToAction(nameof(Index));
        }



        private async Task SaveStockInDetails(int stockInId, List<StockInProductItem> products)
        {
            foreach (var item in products)
            {
                _context.StockInDetails.Add(new StockInDetail
                {
                    StockInId = stockInId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PurchasePrice = item.PurchasePrice,
                    Total = item.Total
                });

                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null)
                    continue;

                product.Quantity += item.Quantity;
                _context.Products.Update(product);
            }
        }
        private async Task<string?> ValidateProducts(List<StockInProductItem> products)
        {
            foreach (var item in products)
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null)
                    return $"Product with ID {item.ProductId} does not exist.";

                if (item.Quantity <= 0)
                    return $"Quantity must be greater than 0 for {item.ProductName}.";
            }

            return null;
        }

        private List<StockInProductItem>? GetProducts(string productsJson)
        {
            if (string.IsNullOrWhiteSpace(productsJson))
                return null;
            return JsonSerializer.Deserialize<List<StockInProductItem>>(
                    productsJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
        }

        private async Task<IActionResult> ReturnWithError(StockInViewModel model, string message)
        {
            ModelState.AddModelError("", message);
            await LoadViewBags(model);
            return View(model);
        }

        private async Task LoadViewBags(StockInViewModel model)
        {
            ViewBag.Suppliers = new SelectList(await _context.Suppliers.ToListAsync(), "Id", "Name", model.SupplierId);
            ViewBag.Products = await _context.Products.ToListAsync();
        }
        public async Task<IActionResult> Details(int id)
        {
            var stockIn = await _context.StockIns
                .Include(s => s.Supplier)
                .Include(s => s.StockInDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (stockIn == null)
            {
                return NotFound();
            }

            return View(stockIn);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var stockIn = await _context.StockIns
                .Include(s => s.Supplier)
                .Include(s => s.StockInDetails)
                   .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (stockIn == null)
            {
                return NotFound();
            }

            return View(stockIn);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockIn = await _context.StockIns
                .Include(s => s.StockInDetails)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stockIn != null)
            {
                foreach (var detail in stockIn.StockInDetails!)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product != null)
                    {
                        product.Quantity -= detail.Quantity;
                        _context.Update(product);
                    }
                }

                _context.StockIns.Remove(stockIn);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Stock In record deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}