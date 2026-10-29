using System.ComponentModel.DataAnnotations;

namespace POS_Management_System.Models
{
    public class Break
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<CashierBreak> CashierBreak { get; set; } = new List<CashierBreak>();

    }
}
