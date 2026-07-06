using System.Collections.Generic;

namespace POS_Management_System.Models.ViewModels
{
    public class PermissionTypeItem
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool Selected { get; set; }
    }

    public class PermissionGroupItem
    {
        public string Name { get; set; }
        public List<PermissionTypeItem> Types { get; set; } = new();
    }

    public class RolePermissionsViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionGroupItem> PermissionGroups { get; set; } = new();
    }
}
