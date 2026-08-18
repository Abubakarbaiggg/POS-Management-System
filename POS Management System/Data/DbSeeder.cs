using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Models;

namespace POS_Management_System.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await context.Database.MigrateAsync();

            string[] roles = {"Admin","Manager","Cashier"};

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string[] permissions =
            {
                "Dashboard View",
                "Product View",
                "Product Create",
                "Product Edit",
                "Product Delete",
                "Category View",
                "Category Create",
                "Category Edit",
                "Category Delete",
                "Customer View",
                "Customer Create",
                "Customer Edit",
                "Customer Delete",
                "Supplier View",
                "Supplier Create",
                "Supplier Edit",
                "Supplier Delete",
                "StockIn View",
                "StockIn Create",
                "StockIn Edit",
                "StockIn Delete",
                "StockOut View",
                "StockOut Create",
                "StockOut Edit",
                "StockOut Delete",
                "Return View",
                "Return Create",
                "Report View",
                "Report Export",
                "User View",
                "User Create",
                "User Edit",
                "User Delete",
                "Role View",
                "Role Create",
                "Role Edit",
                "Role Delete",
                "Permission View",
                "Permission Assign"
            };

            foreach (var permissionName in permissions)
            {
                var exists = await context.Permissions.AnyAsync(x => x.Name == permissionName);
                if (!exists)
                {
                    context.Permissions.Add(new Permission { Name = permissionName , Type = "Permission" });
                }
            }

            await context.SaveChangesAsync();

            await AssignPermissionsToRole(roleManager,"Admin",permissions);

            string[] managerPermissions =
            {
                "Dashboard View",
                "Product View",
                "Product Create",
                "Product Edit",
                "Category View",
                "Category Create",
                "Category Edit",
                "Customer View",
                "Customer Create",
                "Customer Edit",
                "Supplier View",
                "Supplier Create",
                "Supplier Edit",
                "StockIn View",
                "StockIn Create",
                "StockIn Edit",
                "StockOut View",
                "StockOut Create",
                "StockOut Edit",
                "Return View",
                "Return Create",
                "Report View",
                "Report Export"
            };

            await AssignPermissionsToRole(roleManager,"Manager",managerPermissions);

            string[] cashierPermissions =
            {
                "Dashboard View",
                "Product View",
                "Customer View",
                "Customer Create",
                "StockOut View",
                "StockOut Create",
                "Return View",
                "Return Create"
            };

            await AssignPermissionsToRole( roleManager, "Cashier", cashierPermissions);

            string adminEmail = "admin@pos.com";
            string adminPassword = "Admin@12345";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser,adminPassword);
            }

            if (!await userManager.IsInRoleAsync( adminUser,"Admin"))
            {
                var result = await userManager.AddToRoleAsync(
                    adminUser, "Admin"
                );
            }
        }

        private static async Task AssignPermissionsToRole( RoleManager<IdentityRole> roleManager, string roleName, string[] permissionNames)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            var existingClaims = await roleManager.GetClaimsAsync(role);

            foreach (var permissionName in permissionNames)
            {
                var exists = existingClaims.Any(x =>
                    x.Type == "Permission" &&
                    x.Value == permissionName
                );

                if (!exists)
                {
                    var result = await roleManager.AddClaimAsync( role,
                        new Claim("Permission", permissionName)
                    );
                }
            }
        }
    }
}