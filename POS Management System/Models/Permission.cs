using System.ComponentModel.DataAnnotations;

namespace POS_Management_System.Models
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Permission Name")]
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        // Description removed per request
    }
}
