// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Indexes;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Tests.Db;

namespace Suteki.TardisBank.Tests.Spikes
{
    [TestFixture]
    public class RavenDbSchedulingWithIndex : LocalClientTest
    {
        IDocumentStore store;
        Parent parent;
        DateTime now;

        [SetUp]
        public void SetUp()
        {
            store = NewDocumentStore();
            IndexCreation.CreateIndexes(typeof(Child_ByName).Assembly, store);
            parent = new Parent("Dad", "mike@mike.com", "xxx");

            now = new DateTime(2010, 4, 5);

            // create some children with accounts and schedules
            using (var session = store.OpenSession())
            {
                session.Store(CreateChildWithSchedule("one", 1M, now.AddDays(-2)));
                session.Store(CreateChildWithSchedule("two", 2M, now.AddDays(-1)));
                session.Store(CreateChildWithSchedule("three", 3M, now));
                session.Store(CreateChildWithSchedule("four", 4M, now.AddDays(1)));
                session.Store(CreateChildWithSchedule("five", 5M, now.AddDays(2)));
                session.SaveChanges();
            }
        }

        [Test]
        public void Select_child_using_ByName_index()
        {
            using (var session = store.OpenSession())
            {
                var results = session.Advanced.LuceneQuery<Child>("Child/ByName").WhereEquals("Name", "two").WaitForNonStaleResults().ToList();
                results.Count().ShouldEqual(1);
                results[0].Name.ShouldEqual("two");
            }
        }

        [Test]
        public void Select_child_using_ByPendingSchedule()
        {
            using (var session = store.OpenSession())
            {
                var results = session.Advanced
                    .LuceneQuery<Child>("Child/ByPendingSchedule")
                    .WhereLessThan("NextRun", now)
                    .WaitForNonStaleResults().ToList();

                results.Count().ShouldEqual(2);
                results[0].Name.ShouldEqual("one");
                results[1].Name.ShouldEqual("two");
            }
        }

        Child CreateChildWithSchedule(string name, decimal amount, DateTime startDate)
        {
            var child = parent.CreateChild(name, name, "xxx");
            child.Account.AddPaymentSchedule(startDate, Interval.Week, amount, "Pocket Money");
            return child;
        }
        
    }

    public class Child_ByName : AbstractIndexCreationTask<Child>
    {
        public Child_ByName()
        {
            Map = children => from child in children
                              select new { child.Name };
        }
    }

    public class Child_ByPendingSchedule : AbstractIndexCreationTask<Child>
    {
        public Child_ByPendingSchedule()
        {
            Map = children => from child in children
                              from schedule in child.Account.PaymentSchedules
                              select new { schedule.NextRun };
        }
    }


}
// ReSharper restore InconsistentNaming