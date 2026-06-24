namespace POS_Management_System.Models
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
        public string FromName { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
