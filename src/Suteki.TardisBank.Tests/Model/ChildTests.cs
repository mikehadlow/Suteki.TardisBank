// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Tests.Db;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class ChildTests : LocalClientTest
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
        public void Should_be_able_to_create_and_retrieve_a_child()
        {
            var childId = GetChildId();

            using (var session = store.OpenSession())
            {
                var child = session.Load<Child>(childId);
                child.Name.ShouldEqual("Leo");
                child.UserName.ShouldEqual("leohadlow");
                child.Id.ShouldEqual("users/leohadlow");
                child.ParentId.ShouldEqual("users/mike@yahoo.com");
                child.Password.ShouldEqual("xxx");
                child.Account.ShouldNotBeNull();
            }
        }

        string GetChildId()
        {
            using (var session = store.OpenSession())
            {
                var parent = new Parent("Mike Hadlow", "mike@yahoo.com");
                var child = parent.CreateChild("Leo", "leohadlow", "xxx");
                session.Store(child);
                session.SaveChanges();
                return child.Id;
            }  
        }
    }
}
// ReSharper restore InconsistentNaming