using System.Linq;
using Castle.Core;
using Castle.MicroKernel;
using Castle.Windsor;
using NUnit.Framework;
using Raven.Client;
using Suteki.TardisBank.IoC;

namespace Suteki.TardisBank.Tests.Container
{
    [TestFixture]
    public class RavenRegistrationTests
    {
        private IWindsorContainer container;


        [SetUp]
        public void SetUp()
        {
            container = new WindsorContainer().Install(new RavenInstaller());
        }

        [Test]
        public void DocumentStore_is_singleton()
        {
            var store = GetHandler<IDocumentStore>();

            Assert.IsNotNull(store);
            Assert.AreEqual(LifestyleType.Singleton, store.ComponentModel.LifestyleType);
        }

        [Test]
        public void DocumentSession_is_Per_web_request()
        {
            var session = GetHandler<IDocumentSession>();

            Assert.IsNotNull(session);
            Assert.AreEqual(LifestyleType.PerWebRequest, session.ComponentModel.LifestyleType);
        }

        [Test]
        public void DocumentSession_has_first_SaveChanges_decommision_concern()
        {
            var session = GetHandler<IDocumentSession>();

            var concern = session.ComponentModel.Lifecycle.DecommissionConcerns.FirstOrDefault();
            Assert.IsNotNull(concern);
            Assert.IsInstanceOf<SaveDocumentSessionChangesConcern>(concern);
        }

        private IHandler GetHandler<T>()
        {
            return container.Kernel.GetHandler(typeof (T));
        }
    }

}