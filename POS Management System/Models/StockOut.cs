using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_Management_System.Models
{
    public class StockOut
    {
        public int Id { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        //public virtual ICollection<Payment>? Payments { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual ICollection<StockOutDetail>? StockOutDetails { get; set; }
    }
}
