using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Suteki.TardisBank.IoC
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes
                    .FromThisAssembly()
                    .Where(Component.IsInNamespace("Suteki.TardisBank.Services"))
                    .WithService.DefaultInterface()
                    .Configure(c => c.LifeStyle.Transient)
                );
        }
    }
}