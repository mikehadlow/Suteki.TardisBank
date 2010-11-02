using System;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Events
{
    public class SendMessageEvent : IDomainEvent
    {
        public User User { get; private set; }
        public string Message { get; private set; }

        public SendMessageEvent(User user, string message)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            User = user;
            Message = message;
        }
    }
}