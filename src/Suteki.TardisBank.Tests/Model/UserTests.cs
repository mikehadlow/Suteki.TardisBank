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
    public class UserTests : LocalClientTest
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
        public void Should_be_able_to_treat_Parents_and_Children_Polymorphically()
        {
            using (var session = store.OpenSession())
            {
                var mike = new Parent("Mike Hadlow", "mike@yahoo.com", "yyy");
                var leo = mike.CreateChild("Leo", "leohadlow", "xxx");
                var yuna = mike.CreateChild("Yuna", "yunahadlow", "xxx");
                var john = new Parent("John Robinson", "john@gmail.com", "yyy");
                var jim = john.CreateChild("Jim", "jimrobinson", "xxx");

                session.Store(mike);
                session.Store(leo);
                session.Store(yuna);
                session.Store(john);
                session.Store(jim);
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var users = session.Query<User>().ToArray();

                users.Length.ShouldEqual(5);

                users[0].Id.ShouldEqual("users/mike@yahoo.com");
                users[1].Id.ShouldEqual("users/leohadlow");
                users[2].Id.ShouldEqual("users/yunahadlow");
                users[3].Id.ShouldEqual("users/john@gmail.com");
                users[4].Id.ShouldEqual("users/jimrobinson");

                users[0].GetType().Name.ShouldEqual("Parent");
                users[1].GetType().Name.ShouldEqual("Child");
                users[2].GetType().Name.ShouldEqual("Child");
                users[3].GetType().Name.ShouldEqual("Parent");
                users[4].GetType().Name.ShouldEqual("Child");
            }
        }

        [Test]
        public void Should_be_able_to_delete_a_user()
        {
            string userId = null;
            using (var session = store.OpenSession())
            {
                var user = new Parent("Mike", "mike@mike.com", "xxxx");
                session.Store(user);
                session.SaveChanges();
                userId = user.Id;
            }

            using (var session = store.OpenSession())
            {
                session.Advanced.DatabaseCommands.Delete(userId, null);
                session.SaveChanges();
            }

            using (var session = store.OpenSession())
            {
                var user = session.Load<User>(userId);
                user.ShouldBeNull();
            }
        }
    }
}
// ReSharper restore InconsistentNaming