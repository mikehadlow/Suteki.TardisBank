using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Suteki.TardisBank.IoC
{
    public class MvcGooInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                    Component.For<IControllerFactory>().ImplementedBy<WindsorControllerFactory>().LifeStyle.Singleton,
                    Component.For<TardisConfiguration>().Named("TardisConfiguration").LifeStyle.Singleton
                );
        }
    }
}