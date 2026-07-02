using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using POS_Management_System.Helpers;

namespace POS_Management_System.Controllers
{
    [Authorize]

    public class PermissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionsController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            int pageSize = 10;
            var perms = await PaginatedList<Permission>.CreateAsync(
                _context.Permissions.OrderBy(p => p.Name),
                page,pageSize);
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
