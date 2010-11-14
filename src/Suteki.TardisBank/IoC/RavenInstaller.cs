using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Suteki.TardisBank.Indexes;

namespace Suteki.TardisBank.IoC
{
    public class RavenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDocumentStore>().ImplementedBy<DocumentStore>()
                    .DependsOn(new { connectionStringName = "tardisConnection" })
                    .OnCreate(DoInitialisation)
                    .LifeStyle.Singleton,
                Component.For<IDocumentSession>()
                    .UsingFactoryMethod(GetDocumentSesssion)
                    .LifeStyle.PerWebRequest
                );
        }

        static IDocumentSession GetDocumentSesssion(IKernel kernel)
        {
            var store = kernel.Resolve<IDocumentStore>();
            return store.OpenSession();
        }

        public static void DoInitialisation(IKernel kernel, IDocumentStore store)
        {
            store.Initialize();
            IndexCreation.CreateIndexes(typeof(Child_ByPendingSchedule).Assembly, store);
        }
    }
}