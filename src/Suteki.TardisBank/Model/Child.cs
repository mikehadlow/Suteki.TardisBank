using System;

namespace Suteki.TardisBank.Model
{
    public class Child : User
    {
        public Child(string name, string userName, string password, string parentId) : base(name, userName)
        {
            Password = password;
            ParentId = parentId;
            Account = new Account();
        }

        public string Password { get; set; }
        public string ParentId { get; set; }
        public Account Account { get; set; }

        public void ReceivePayment(decimal amount, string description)
        {
            Account.AddTransaction(description, amount);
        }
    }
}