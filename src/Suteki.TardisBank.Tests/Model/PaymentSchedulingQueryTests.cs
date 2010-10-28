// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Tests.Db;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class PaymentSchedulingQueryTests : LocalClientTest
    {
        IDocumentStore store;
        Parent parent;

        [SetUp]
        public void SetUp()
        {
            store = NewDocumentStore();
            parent = new Parent("Dad", "mike@mike.com", "xxx");
        }

        [Test, Ignore("Need to work out how to do this ...")]
        public void Should_be_able_to_query_all_pending_scheduled_payments()
        {
            var someDate = new DateTime(2010, 4, 5);

            using(var session = store.OpenSession())
            {
                session.Store(CreateChildWithSchedule("one", 1M, someDate.AddDays(-2)));
                session.Store(CreateChildWithSchedule("two", 2M, someDate.AddDays(-1)));
                session.Store(CreateChildWithSchedule("three", 3M, someDate));
                session.Store(CreateChildWithSchedule("four", 4M, someDate.AddDays(1)));
                session.Store(CreateChildWithSchedule("five", 5M, someDate.AddDays(2)));
            }

            using(var session = store.OpenSession())
            {
                var results = session.Query<Child>("pendingScheduledPayments")
                    .Where(child => child.Account.PaymentSchedules.Where(s => s.NextRun <= someDate).Any());

                foreach (var child in results)
                {
                    Console.WriteLine("{0} - {1}",
                        child.Name,
                        child.Account.PaymentSchedules[0].NextRun);
                }
            }
        }

        Child CreateChildWithSchedule(string name, decimal amount, DateTime startDate)
        {
            var child = parent.CreateChild(name, name, "xxx");
            child.Account.AddPaymentSchedule(startDate, Interval.Week, amount, "Pocket Money");
            return child;
        }
    }
}
// ReSharper restore InconsistentNaming