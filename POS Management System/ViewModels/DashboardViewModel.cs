namespace POS_Management_System.ViewModels
{
    public class DashboardViewModel
    {
        public int ProductsCount { get; set; }
        public int LowStockCount { get; set; }

        public int SuppliersCount { get; set; }
        public int CustomersCount { get; set; }

        public decimal TodaySales { get; set; }
        public decimal MonthlySales { get; set; }

        public decimal MonthlyStockIn { get; set; }

        public decimal TotalStockIn { get; set; }
        public decimal TotalStockOut { get; set; }
    }
}
