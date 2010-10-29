using System;
using System.Net;
using System.Net.Mail;
using Suteki.TardisBank.IoC;

namespace Suteki.TardisBank.Services
{
    public interface IEmailService
    {
        void SendEmail(string toAddress, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        readonly TardisConfiguration configuration;

        public EmailService(TardisConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmail(string toAddress, string subject, string body)
        {
            if (toAddress == null)
            {
                throw new ArgumentNullException("toAddress");
            }
            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }
            if (string.IsNullOrWhiteSpace(configuration.EmailSmtpServer)) return;

            var message = new MailMessage(
                        configuration.EmailFromAddress,
                        toAddress,
                        subject,
                        body);

            var client = new SmtpClient(configuration.EmailSmtpServer)
            {
                EnableSsl = configuration.EmailEnableSsl,
                Port = configuration.EmailPort,
            };

            if (!string.IsNullOrWhiteSpace(configuration.EmailCredentialsUserName) && 
                !string.IsNullOrWhiteSpace(configuration.EmailCredentialsPassword))
            {
                client.Credentials = new NetworkCredential(configuration.EmailCredentialsUserName,
                                                           configuration.EmailCredentialsPassword);
            }

            client.Send(message);            
        }
    }
}