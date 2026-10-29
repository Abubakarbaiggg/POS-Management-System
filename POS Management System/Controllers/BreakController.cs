using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;

namespace POS_Management_System.Controllers
{
    public class BreakController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BreakController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Breaks = await _context.Breaks.ToListAsync();
            return View(Breaks);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Break breaks)
        {
            if (ModelState.IsValid)
            {
                _context.Breaks.Add(breaks);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(breaks);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var breaks = await _context.Breaks.FindAsync(id);
            if (breaks == null)
            {
                return NotFound();
            }
            return View(breaks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Break breaks)
        {
            if (id != breaks.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Update(breaks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(breaks);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var breaks = await _context.Breaks.FindAsync(id);
            if (breaks != null)
            {
                _context.Breaks.Remove(breaks);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
