using System.ComponentModel.DataAnnotations;

namespace POS_Management_System.Models.ViewModels
{
    public class StockInViewModel
    {
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public virtual Supplier? Supplier { get; set; }

        public List<StockInProductItem> Products { get; set; } = new List<StockInProductItem>();

        public decimal TotalAmount { get; set; }
    }

    public class StockInProductItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Total { get; set; }
    }
}