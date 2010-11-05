using System;
using System.Linq;
using System.Collections.Generic;

namespace Suteki.TardisBank.Model
{
    public class Account
    {
        public const int MaxTransactions = 100;
        public decimal OldTransactionsBalance { get; private set; }

        public Account()
        {
            Transactions = new List<Transaction>();
            PaymentSchedules = new List<PaymentSchedule>();
            OldTransactionsBalance = 0M;
        }

        public IList<Transaction> Transactions { get; private set; }

        public decimal Balance
        {
            get { return OldTransactionsBalance + Transactions.Sum(x => x.Amount); }
        }

        public IList<PaymentSchedule> PaymentSchedules { get; private set; }

        public void AddTransaction(string description, decimal amount)
        {
            Transactions.Add(new Transaction(description, amount));

            RemoveOldTransactions();
        }

        void RemoveOldTransactions()
        {
            if (Transactions.Count <= MaxTransactions) return;

            var oldestTransaction = Transactions.First();
            Transactions.Remove(oldestTransaction);
            OldTransactionsBalance += oldestTransaction.Amount;
        }

        public void AddPaymentSchedule(DateTime startDate, Interval interval, decimal amount, string description)
        {
            var nextId = PaymentSchedules.Any() ? PaymentSchedules.Max(x => x.Id) + 1 : 0;
            PaymentSchedules.Add(new PaymentSchedule(nextId, startDate, interval, amount, description));
        }

        public void TriggerScheduledPayments(DateTime now)
        {
            var overdueSchedules = PaymentSchedules.Where(x => x.NextRun <= now);
            foreach (var overdueSchedule in overdueSchedules)
            {
                AddTransaction(overdueSchedule.Description, overdueSchedule.Amount);
                overdueSchedule.CalculateNextRunDate();
            }
        }

        public void RemovePaymentSchedule(int paymentScheduleId)
        {
            var scheduleToRemove = PaymentSchedules.SingleOrDefault(x => x.Id == paymentScheduleId);
            if (scheduleToRemove == null) return;

            PaymentSchedules.Remove(scheduleToRemove);
        }
    }
}