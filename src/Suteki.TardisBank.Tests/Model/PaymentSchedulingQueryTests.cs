// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Services;
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

        [Test]
        public void Should_be_able_to_query_all_pending_scheduled_payments()
        {
            var someDate = new DateTime(2010, 4, 5);

            // create some children with accounts and schedules
            using(var session = store.OpenSession())
            {
                session.Store(CreateChildWithSchedule("one", 1M, someDate.AddDays(-2)));
                session.Store(CreateChildWithSchedule("two", 2M, someDate.AddDays(-1)));
                session.Store(CreateChildWithSchedule("three", 3M, someDate));
                session.Store(CreateChildWithSchedule("four", 4M, someDate.AddDays(1)));
                session.Store(CreateChildWithSchedule("five", 5M, someDate.AddDays(2)));
                session.SaveChanges();
            }

            // perform the update
            using(var session = store.OpenSession())
            {
                ISchedulerService schedulerService = new SchedulerService(session);
                schedulerService.ExecuteUpdates(someDate);
                session.SaveChanges();
            }

            // check results
            using(var session = store.OpenSession())
            {
                var results = session.Query<Child>().ToList();

                results.Count().ShouldEqual(5);

                results.Single(x => x.Name == "one").Account.PaymentSchedules[0].NextRun.ShouldEqual(someDate.AddDays(5));
                results.Single(x => x.Name == "two").Account.PaymentSchedules[0].NextRun.ShouldEqual(someDate.AddDays(6));
                results.Single(x => x.Name == "three").Account.PaymentSchedules[0].NextRun.ShouldEqual(someDate.AddDays(7));
                results.Single(x => x.Name == "four").Account.PaymentSchedules[0].NextRun.ShouldEqual(someDate.AddDays(1));
                results.Single(x => x.Name == "five").Account.PaymentSchedules[0].NextRun.ShouldEqual(someDate.AddDays(2));

                results.Single(x => x.Name == "one").Account.Transactions.Count.ShouldEqual(1);
                results.Single(x => x.Name == "one").Account.Transactions[0].Amount.ShouldEqual(1M);
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