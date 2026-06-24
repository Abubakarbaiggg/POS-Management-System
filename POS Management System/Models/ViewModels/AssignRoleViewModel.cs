using Microsoft.AspNetCore.Mvc.Rendering;

namespace POS_Management_System.Models.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string SelectedRole { get; set; }

        public List<SelectListItem> Roles { get; set; } = new();
    }
}
