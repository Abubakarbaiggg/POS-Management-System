using System.Collections.Generic;

namespace POS_Management_System.Models.ViewModels
{
    public class RolePermissionsViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionItem> Permissions { get; set; } = new List<PermissionItem>();
    }
}
