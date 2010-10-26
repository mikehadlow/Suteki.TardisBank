// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.Tests.Db;

namespace Suteki.TardisBank.Tests.Spikes
{
    [TestFixture]
    public class RavenDbSpikes : LocalClientTest
    {
        IDocumentStore store;

        [SetUp]
        public void SetUp()
        {
            store = NewDocumentStore();
        }

        [TearDown]
        public void TearDown()
        {
            if (store != null)
            {
                store.Dispose();
            }
        }

        [Test]
        public void First_Raven_Client_test()
        {
            using (var session = store.OpenSession())
            {
                var product = new Product
                {
                    Cost = 3.99m,
                    Name = "Milk",
                };
                session.Store(product);
                session.SaveChanges();

                session.Store(new Order
                {
                    Customer = "customers/ayende",
                    OrderLines =
                    {
                        new OrderLine
                        {
                            ProductId = product.Id,
                            Quantity = 3
                        },
                    }
                });
                session.SaveChanges();
            }
        }        
    }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }

    public class Order
    {
        public string Id { get; set; }
        public string Customer { get; set; }
        public IList<OrderLine> OrderLines { get; set; }

        public Order()
        {
            OrderLines = new List<OrderLine>();
        }
    }

    public class OrderLine
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
// ReSharper restore InconsistentNaming