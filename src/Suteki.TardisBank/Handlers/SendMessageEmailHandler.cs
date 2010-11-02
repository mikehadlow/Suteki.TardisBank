using System;
using Suteki.TardisBank.Events;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Services;

namespace Suteki.TardisBank.Handlers
{
    public class SendMessageEmailHandler : IHandle<SendMessageEvent>
    {
        readonly IEmailService emailService;

        public SendMessageEmailHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void Handle(SendMessageEvent sendMessageEvent)
        {
            if (sendMessageEvent == null)
            {
                throw new ArgumentNullException("sendMessageEvent");
            }

            if (sendMessageEvent.User is Child)
            {
                // we cannot send email messages to children.
                return;
            }

            var toAddress = sendMessageEvent.User.UserName;
            const string subject = "Message from Tardis Bank";
            var body = sendMessageEvent.Message;

            emailService.SendEmail(toAddress, subject, body);
        }
    }
}