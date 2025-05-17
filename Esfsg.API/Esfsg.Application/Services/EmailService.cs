using Esfsg.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Esfsg.Application.Services
{
    public class EmailService : IEmailService
    {

        #region Constructor
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string password = string.Empty;
        private readonly string user = string.Empty;
        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            password = _configuration["EmailSettings:FromPassword"] ?? throw new ArgumentException("Destinatário não encontrado");
            user = _configuration["EmailSettings:FromAddress"] ?? throw new ArgumentException("Destinatário não encontrado");
            _logger = logger;
        }
        #endregion

       
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress(user);
                var toAddress = new MailAddress(to);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, password)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao enviar o email: {e.Message}");
                throw;
            }
        }

    }
}
