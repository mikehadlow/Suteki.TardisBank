// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Tests.Db;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class MessageTests : LocalClientTest
    {
        User user;
        IDocumentStore store;

        [SetUp]
        public void SetUp()
        {
            store = NewDocumentStore();
            user = new Parent("Dad", "mike@mike.com", "xxx");
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
        public void Should_be_able_to_add_a_message_to_a_user()
        {
            string userId;
            using(var session = store.OpenSession())
            {
                session.Store(user);
                session.SaveChanges();
                userId = user.Id;
            }

            using(var session = store.OpenSession())
            {
                var parent = session.Load<Parent>(userId);
                parent.SendMessage("some message");

                parent.Messages.Count.ShouldEqual(1);
                session.SaveChanges();
            }

            using(var session = store.OpenSession())
            {
                var parent = session.Load<Parent>(userId);
                parent.Messages.Count.ShouldEqual(1);
            }
        }
    }
}
// ReSharper restore InconsistentNaming