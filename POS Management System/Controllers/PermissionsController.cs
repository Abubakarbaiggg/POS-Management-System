using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;

namespace POS_Management_System.Controllers
{
    [Authorize]

    public class PermissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PermissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var perms = await _context.Permissions.OrderBy(p => p.Name).ToListAsync();
            return View(perms);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Permission permission)
        {
            if (!ModelState.IsValid) return View(permission);

            // ensure unique name
            if (await _context.Permissions.AnyAsync(p => p.Name == permission.Name))
            {
                ModelState.AddModelError("Name", "A permission with this name already exists.");
                return View(permission);
            }

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Permission created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return NotFound();
            return View(permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Permission permission)
        {
            if (id != permission.Id) return BadRequest();
            if (!ModelState.IsValid) return View(permission);

            if (await _context.Permissions.AnyAsync(p => p.Name == permission.Name && p.Id != id))
            {
                ModelState.AddModelError("Name", "A permission with this name already exists.");
                return View(permission);
            }

            _context.Update(permission);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Permission updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return NotFound();
            return View(permission);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return NotFound();

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Permission deleted.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return NotFound();
            return View(permission);
        }
    }
}
