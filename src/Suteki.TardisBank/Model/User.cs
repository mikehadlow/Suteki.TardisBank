using System;
using System.Collections.Generic;
using System.Linq;

namespace Suteki.TardisBank.Model
{
    public abstract class User
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public IList<Message> Messages { get; private set; }

        protected User(string name, string userName, string password)
        {
            Id = UserIdFromUserName(userName);
            Name = name;
            UserName = userName;
            Password = password;
            Messages = new List<Message>();
        }

        public static string UserIdFromUserName(string userName)
        {
            return string.Format("users/{0}", userName);
        }

        public void SendMessage(string text)
        {
            var nextId = Messages.Count == 0 ? 0 : Messages.Max(x => x.Id) + 1;
            Messages.Add(new Message(nextId, DateTime.Now.Date, text));
        }

        public void ReadMessage(int messageId)
        {
            var message = Messages.SingleOrDefault(x => x.Id == messageId);
            if (message == null)
            {
                throw new TardisBankException("No message with Id {0} found for user '{1}'", messageId, UserName);
            }
            message.Read();
        }
    }
}