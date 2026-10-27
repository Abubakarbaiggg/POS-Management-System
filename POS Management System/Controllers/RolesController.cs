using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using System.Data;
using System.Security.Claims;
using POS_Management_System.Models;
using POS_Management_System.Models.ViewModels;

namespace POS_Management_System.Controllers
{
    [Authorize]

    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "Role name is required.");
                return View();
            }

            if (await _roleManager.RoleExistsAsync(name))
            {
                ModelState.AddModelError("Name", "A role with this name already exists.");
                return View();
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                TempData["Success"] = "Role created successfully.";
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();

        }

       public async Task<IActionResult> ManagePermissions(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            var allPermissions = await _context.Permissions
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Type)
                .ToListAsync();

            var assignedPermissions = (await _roleManager.GetClaimsAsync(role))
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToHashSet();

            var model = new RolePermissionsViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name!,

                PermissionGroups = allPermissions
                    .Select(p =>
                    {
                        var lastSpace = p.Name.LastIndexOf(' ');

                        var groupName = lastSpace > 0
                            ? p.Name[..lastSpace]
                            : p.Name;

                        var permissionType = lastSpace > 0
                            ? p.Name[(lastSpace + 1)..]
                            : p.Name;

                        return new
                        {
                            Permission = p,
                            GroupName = groupName,
                            PermissionType = permissionType
                        };
                    })
                    .GroupBy(x => x.GroupName)
                    .Select(g => new PermissionGroupItem
                    {
                        Name = g.Key,

                        Types = g.Select(x => new PermissionTypeItem
                        {
                            Id = x.Permission.Id,
                            Type = x.PermissionType,

                            Selected = assignedPermissions.Contains(
                                $"{x.Permission.Name} {x.Permission.Type}"
                            )

                        }).ToList()

                    })
                    .ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRolePermissions(string roleId, int[]? selectedPermissionIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role == null) return NotFound();

            var existingClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in existingClaims.Where(c => c.Type == "Permission"))
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            if (selectedPermissionIds != null && selectedPermissionIds.Any())
            {
                var permissions = await _context.Permissions
                    .Where(p => selectedPermissionIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var permission in permissions)
                {
                    await _roleManager.AddClaimAsync(
                        role,
                        new Claim("Permission", $"{permission.Name} {permission.Type}")
                    );
                }
            }
            TempData["Success"] = "Role permissions updated successfully.";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string name)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "Role name is required.");
                return View(role);
            }

            var existing = await _roleManager.FindByNameAsync(name);
            if (existing != null && existing.Id != id)
            {
                ModelState.AddModelError("Name", "A role with this name already exists.");
                return View(role);
            }

            role.Name = name;
            role.NormalizedName = name.ToUpperInvariant();
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = "Role updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(role);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = "Role deleted successfully.";
            }
            else
            {
                TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Description));
            }
                
            return RedirectToAction(nameof(Index));
        }
    }
}
