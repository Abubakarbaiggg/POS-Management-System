namespace POS_Management_System.Models
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSSL { get; set; }
        public string FromName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
    }
}
