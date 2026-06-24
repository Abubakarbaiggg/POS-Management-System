using Microsoft.Extensions.Options;
using POS_Management_System.Models;
using System.Net;
using System.Net.Mail;

namespace POS_Management_System.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendLowStockAlertAsync(string productName, int stock)
        {
            Console.WriteLine("Email method started");
            var message = new MailMessage
            {
                From = new MailAddress(
                  _settings.SenderEmail,
                  _settings.FromName
                 ),
                Subject = "Low Stock Alert",
                Body = $@"
                    Product Name : {productName}
                    Current Stock : {stock}

                    Please refill stock.
                    ",
                IsBodyHtml = false

            };
            message.To.Add(_settings.ReceiverEmail);
            using var smtp = new SmtpClient(
                 _settings.SmtpServer,
                 _settings.Port
             );

            smtp.Credentials = new NetworkCredential(
                _settings.SenderEmail,
                _settings.Password
            );

            smtp.EnableSsl = _settings.EnableSSL;

            await smtp.SendMailAsync(message);
            Console.WriteLine("Email sent successfully");
        }
    }
}
