using System;
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
                    .Where(type => type.Namespace == "Suteki.TardisBank.Services")
                    .WithService.FirstInterface()
                    .Configure(c => c.LifeStyle.Transient)
                );
        }
    }
}