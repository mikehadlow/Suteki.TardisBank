using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.Tests.Db;

namespace Suteki.TardisBank.Tests.Spikes
{
    [TestFixture]
    public class RavenDbAnyOfPropertyCollection : LocalClientTest
    {
        IDocumentStore store;

        [SetUp]
        public void SetUp()
        {
            store = NewDocumentStore();
            using(var session = store.OpenSession())
            {
                session.Store(new Account
                {
                    Transactions =
                        {
                            new Transaction(1),
                            new Transaction(3),
                        }
                });
                session.Store(new Account
                {
                    Transactions =
                        {
                            new Transaction(2),
                            new Transaction(4),
                        }
                });

                session.SaveChanges();
            }
        }

        // works as expected
        [Test]
        public void ShouldBeAbleToQueryOnTransactionAmount()
        {
            using(var session = store.OpenSession())
            {
                var accounts = session.Query<Account>()
                    .Where(x => x.Transactions.Any(y => y.Amount == 2));
                Assert.That(accounts.Count(), Is.EqualTo(1));
            }
        }

        // This test fails, should return two results but actually reurns zero.
        [Test, Explicit]
        public void InequalityOperatorDoesNotWorkOnAny()
        {
            using(var session = store.OpenSession())
            {
                var accounts = session.Query<Account>().Where(x => x.Transactions.Any(y => y.Amount < 3));
                Assert.That(accounts.Count(), Is.EqualTo(2));
            }
        }

        // System.InvalidCastException : Unable to cast object of type 'System.Linq.Expressions.MethodCallExpressionN' to type 'System.Linq.Expressions.MemberExpression'.
        [Test, Explicit]
        public void InequalityOperatorDoesNotWorkOnWhereThenAny()
        {
            using(var session = store.OpenSession())
            {
                var accounts = session.Query<Account>().Where(x => x.Transactions.Where(y => y.Amount <= 2).Any());
                Assert.That(accounts.Count(), Is.EqualTo(2));
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (store != null) store.Dispose();
        }
        
    }

    public class Account
    {
        public Account()
        {
            Transactions = new List<Transaction>();
        }

        public IList<Transaction> Transactions { get; private set; }
    }

    public class Transaction
    {
        public Transaction(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; private set; }
    }
}