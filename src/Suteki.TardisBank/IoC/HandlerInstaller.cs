using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Suteki.TardisBank.Handlers;

namespace Suteki.TardisBank.IoC
{
    public class HandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly()
                    .BasedOn(typeof (IHandle<>)).WithService.Base()
                    .Configure(c => c.LifeStyle.Transient)
                    );
        }
    }
}