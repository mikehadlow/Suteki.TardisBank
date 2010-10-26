using System;
using System.Linq;
using System.Collections.Generic;

namespace Suteki.TardisBank.Model
{
    public class Account
    {
        public Account()
        {
            Transactions = new List<Transaction>();
        }

        public IList<Transaction> Transactions { get; private set; }

        public decimal Balance
        {
            get { return Transactions.Sum(x => x.Amount); }
        }

        public void AddTransaction(string description, decimal amount)
        {
            Transactions.Add(new Transaction(description, amount));
        }
    }
}