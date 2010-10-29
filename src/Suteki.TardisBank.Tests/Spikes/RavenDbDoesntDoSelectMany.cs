using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.Tests.Db;

namespace Suteki.TardisBank.Tests.Spikes
{
    [TestFixture]
    public class RavenDbDoesntDoSelectMany : LocalClientTest
    {
        IDocumentStore store;

        [SetUp]
        public void SetUp()
        {
            store = NewDocumentStore();
            using (var session = store.OpenSession())
            {
                session.Store(new Order
                {
                    OrderLines =
                        {
                            new OrderLine("Widget"),
                            new OrderLine("Gadget")
                        }
                });
                session.Store(new Order
                {
                    OrderLines =
                        {
                            new OrderLine("Fixit"),
                            new OrderLine("Gadget")
                        }
                });
                session.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            if(store != null) store.Dispose();
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TryToSelectMany()
        {
            // failed: System.NotSupportedException : Method not supported: SelectMany
            using(var session = store.OpenSession())
            {
                var orderLines = session.Query<Order>().SelectMany(x => x.OrderLines);
                Assert.That(orderLines.Count(), Is.EqualTo(4));
            }
        }

        [Test]
        public void TryToFindProductsByName()
        {
            using (var session = store.OpenSession())
            {
                var orders = session.Query<Order>().Where(x => x.OrderLines.Any(y => y.Product.Name == "Widget"));
                Assert.That(orders.Count(), Is.EqualTo(1));
            }
        }
    }

    public class Order
    {
        public Order()
        {
            OrderLines = new List<OrderLine>();
        }

        public IList<OrderLine> OrderLines { get; private set; }
    }

    public class OrderLine
    {
        public OrderLine(string productName)
        {
            Product = new Product(productName);
        }

        public Product Product { get; private set; }
    }

    public class Product
    {
        public Product(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}