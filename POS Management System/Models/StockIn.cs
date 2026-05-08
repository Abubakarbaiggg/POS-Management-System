using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_Management_System.Models
{
    public class StockIn
    {
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }

        public virtual ICollection<StockInDetail>? StockInDetails { get; set; }
    }
}
