using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using POS_Management_System.Services.Permission;
using System.Security.Claims;

namespace POS_Management_System.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PermissionService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> HasPermissionAsync(
            ClaimsPrincipal user,
            string permission)
        {
            var appUser = await _userManager.GetUserAsync(user);

            if (appUser == null)
                return false;

            var roleIds = await _context.UserRoles
                .Where(x => x.UserId == appUser.Id)
                .Select(x => x.RoleId)
                .ToListAsync();

            return await _context.RolePermissions
                .Include(x => x.Permission)
                .AnyAsync(x =>
                    roleIds.Contains(x.RoleId) &&
                    x.Permission.Name == permission);
        }
    }
}