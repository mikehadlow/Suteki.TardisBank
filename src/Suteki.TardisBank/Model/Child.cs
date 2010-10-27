using System;
using System.Runtime.Serialization;

namespace Suteki.TardisBank.Model
{
    public class Child : User
    {
        public Child(string name, string userName, string password, string parentId) : base(name, userName, password)
        {
            ParentId = parentId;
            Account = new Account();
        }

        public string ParentId { get; set; }
        public Account Account { get; set; }

        public void ReceivePayment(decimal amount, string description)
        {
            Account.AddTransaction(description, amount);
        }

        public void WithdrawCashFromParent(Parent parent, decimal amount, string description)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            if (!parent.HasChild(this))
            {
                throw new CashWithdrawException("Not Your Parent");
            }

            if (amount > Account.Balance)
            {
                throw new CashWithdrawException(string.Format(
                    "You can not withdraw {0} because you only have {1} in your account", 
                    amount,
                    Account.Balance));
            }

            parent.SendMessage(string.Format("{0} would like to withdraw {1}", Name, amount));
            Account.AddTransaction(description, -amount);
        }
    }

    [Serializable]
    public class CashWithdrawException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CashWithdrawException()
        {
        }

        public CashWithdrawException(string message) : base(message)
        {
        }

        public CashWithdrawException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CashWithdrawException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}