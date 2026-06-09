using System.ComponentModel.DataAnnotations;

namespace POS_Management_System.Models.ViewModels
{
    public class StockOutViewModel
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public List<StockOutProductItem> Products { get; set; } = new List<StockOutProductItem>();

        public decimal TotalAmount { get; set; }
    }

    public class StockOutProductItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}
