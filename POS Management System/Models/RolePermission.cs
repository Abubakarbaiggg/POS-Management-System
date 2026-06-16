using Microsoft.AspNetCore.Identity;

namespace POS_Management_System.Models
{
    public class RolePermission
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public int PermissionId { get; set; }

        public IdentityRole Role { get; set; }
        public Permission Permission { get; set; }
    }
}

