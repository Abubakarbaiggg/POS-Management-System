namespace POS_Management_System.Models.ViewModels
{
    public class PermissionGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = new();
    }
}
