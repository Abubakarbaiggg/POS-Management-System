using System.Collections.Generic;

namespace POS_Management_System.Models.ViewModels
{
    public class PermissionItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class UserPermissionsViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<PermissionItem> Permissions { get; set; } = new List<PermissionItem>();
    }
}
