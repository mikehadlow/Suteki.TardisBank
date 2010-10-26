using System;

namespace Suteki.TardisBank.Model
{
    public class Transaction
    {
        public Transaction(string description, decimal amount)
        {
            Description = description;
            Amount = amount;
            Date = DateTime.Now.Date;
        }

        public string Description { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
    }
}