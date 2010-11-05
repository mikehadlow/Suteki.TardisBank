// ReSharper disable InconsistentNaming
using System.Linq;
using NUnit.Framework;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class TransactionCountLimitTests
    {
        Child child;

        [SetUp]
        public void SetUp()
        {
            child = new Parent("Mike", "mike@mike.com", "xxx").CreateChild("Leo", "leo2", "yyy");
        }

        [Test]
        public void When_more_than_max_transactions_created_transactions_should_be_truncated()
        {
            for (int i = 0; i < Account.MaxTransactions; i++)
            {
                child.ReceivePayment(1M, "payment" + i);
            }

            child.Account.Balance.ShouldEqual(100M);
            child.Account.Transactions.Count.ShouldEqual(Account.MaxTransactions);

            child.ReceivePayment(2M, "payment_new");

            child.Account.Balance.ShouldEqual(102M);
            child.Account.Transactions.Count.ShouldEqual(Account.MaxTransactions);
            child.Account.OldTransactionsBalance.ShouldEqual(1M);

            child.Account.Transactions.First().Description.ShouldEqual("payment1");
            child.Account.Transactions.Last().Description.ShouldEqual("payment_new");

            child.ReceivePayment(3.55M, "payment_new2");

            child.Account.Balance.ShouldEqual(105.55M);
            child.Account.Transactions.Count.ShouldEqual(Account.MaxTransactions);
            child.Account.OldTransactionsBalance.ShouldEqual(2M);

            child.Account.Transactions.First().Description.ShouldEqual("payment2");
            child.Account.Transactions.Last().Description.ShouldEqual("payment_new2");
        }
    }
}
// ReSharper restore InconsistentNaming