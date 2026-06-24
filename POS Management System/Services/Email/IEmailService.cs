namespace POS_Management_System.Services.Email
{
    public interface IEmailService
    {
        Task SendLowStockAlertAsync(string productName, int stock);
    }
}
