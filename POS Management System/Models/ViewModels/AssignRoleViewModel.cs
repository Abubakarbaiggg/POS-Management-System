using Microsoft.AspNetCore.Mvc.Rendering;

namespace POS_Management_System.Models.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string SelectedRole { get; set; } = string.Empty;
        public List<SelectListItem> Roles { get; set; } = new();
    }
}
