// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.Events;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Tests.Db;

namespace Suteki.TardisBank.Tests.Model
{
    [TestFixture]
    public class ParentTests : LocalClientTest
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
        public void Should_be_able_to_create_and_retrieve_Parent()
        {
            var parentId = GetParentId("mike");

            using (var session = store.OpenSession())
            {
                var parent = session.Load<User>(parentId) as Parent;

                parent.ShouldNotBeNull();
                parent.Name.ShouldEqual("Mike Hadlow");
                parent.UserName.ShouldEqual("mike@yahoo.com");
                parent.Id.ShouldEqual("users/mike@yahoo.com");
                parent.Children.ShouldNotBeNull();
            }
        }

        string GetParentId(string name)
        {
            string parentId;

            using (var session = store.OpenSession())
            {
                var parent = new Parent(name: "Mike Hadlow", userName: string.Format("{0}@yahoo.com", name), password: "yyy");
                session.Store(parent);
                session.SaveChanges();
                parentId = parent.Id;
            }
            return parentId;
        }

        [Test]
        public void Should_be_able_to_add_a_child_to_a_parent()
        {
            var parentId = GetParentId("joe");

            using (var session = store.OpenSession())
            {
                var parent = session.Load<Parent>(parentId);

                parent.CreateChild("jim", "jim123", "passw0rd1");
                parent.CreateChild("jenny", "jenny123", "passw0rd2");
                parent.CreateChild("jez", "jez123", "passw0rd3");
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var parent = session.Load<Parent>(parentId);
                parent.Children.Count.ShouldEqual(3);

                parent.Children[0].Name.ShouldEqual("jim");
                parent.Children[1].Name.ShouldEqual("jenny");
                parent.Children[2].Name.ShouldEqual("jez");

                parent.Children[0].ChildId.ShouldEqual("users/jim123");
            }
        }

        [Test]
        public void Cant_retrieve_a_parent_that_doesnt_exist()
        {
            using (var session = store.OpenSession())
            {
                var parent = session.Load<Parent>("users/someUnknownParent");
                parent.ShouldBeNull();
            }
        }
    }
}
// ReSharper restore InconsistentNaming