using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_Management_System.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public int StockOutId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]

        public decimal PaidAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]

        public decimal RemainingAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;
        [StringLength(100)]

        public string? TransactionId { get; set; }
        [StringLength(500)]

        public string? Notes { get; set; }

        public virtual StockOut StockOut { get; set; }
    }
    public enum PaymentMethod
    {
        Cash,
        BankTransfer,
        Card,
        JazzCash,
        EasyPaisa,
        Stripe
    }
}
