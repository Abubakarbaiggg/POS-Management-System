namespace POS_Management_System.Models.ViewModels
{
    public class SaleReportViewModel
    {
        public string? Filter { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<Sale> Sales { get; set; } = new();
    }
}
