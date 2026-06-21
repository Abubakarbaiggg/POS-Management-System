using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using POS_Management_System.Models.ViewModels;
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

        public async Task<IActionResult> Index()
        {
            var stockIns = await _context.StockIns
                .Include(s => s.Supplier)
                .Include(s => s.StockInDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
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
            if (string.IsNullOrEmpty(productsJson))
            {
                ModelState.AddModelError("", "Please add at least one product");
                await LoadViewBags(model);
                return View(model);
            }

            var products = JsonSerializer.Deserialize<List<StockInProductItem>>(productsJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (products == null || products.Count == 0)
            {
                ModelState.AddModelError("", "Please add at least one product");
                await LoadViewBags(model);
                return View(model);
            }

            foreach (var item in products)
            {
                var productExists = await _context.Products.AnyAsync(p => p.Id == item.ProductId);
                if (!productExists)
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

                if (item.PurchasePrice <= 0)
                {
                    ModelState.AddModelError("", $"Purchase price must be greater than 0 for {item.ProductName}");
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

                var stockIn = new StockIn
                {
                    SupplierId = model.SupplierId,
                    Date = model.Date
                };

                _context.StockIns.Add(stockIn);
                await _context.SaveChangesAsync();

                foreach (var item in products)
                {
                    var stockInDetail = new StockInDetail
                    {
                        StockInId = stockIn.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        PurchasePrice = item.PurchasePrice,
                        Total = item.Total
                    };
                    _context.StockInDetails.Add(stockInDetail);

                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Quantity += item.Quantity;
                        _context.Update(product);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = $"Stock In completed successfully! Total amount: {totalAmount:N2}";
                return RedirectToAction(nameof(Index));
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