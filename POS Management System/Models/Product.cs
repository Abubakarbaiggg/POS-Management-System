using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_Management_System.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [StringLength(500)]
        public string? ImagePath { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public virtual ICollection<SaleDetail>? SaleDetails { get; set; }
        public virtual ICollection<StockInDetail>? StockInDetails { get; set; }
    }
}
