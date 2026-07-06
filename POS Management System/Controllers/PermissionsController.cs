using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Helpers;
using POS_Management_System.Models;
using POS_Management_System.Models.ViewModels;
using System.Security.Claims;

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

        public async Task<IActionResult> Index()
        {
            var permissions = await _context.Permissions
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Type)
                .ToListAsync();

            var model = permissions
                .GroupBy(x => x.Name)
                .Select(g => new PermissionGroupViewModel
                {
                    Name = g.Key,
                    Permissions = g.ToList()
                })
                .ToList();

            return View(model);
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

            if (await _context.Permissions.AnyAsync(p => p.Name == permission.Name && p.Type == permission.Type))
            {
                ModelState.AddModelError("Name", "A permission with this name already exists.");
                return View(permission);
            }

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Permission created.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string name, string type)
        {
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(x => x.Name == name && x.Type == type);

            if (permission == null)
                return NotFound();

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{type} permission deleted.";
            return RedirectToAction(nameof(Index));
        }

    }
}
