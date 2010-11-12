using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Raven.Client;

namespace Suteki.TardisBank.IoC
{
    // NOTE: this should be baked into Windsor
    public class SaveChangesOnDecommissionDescriptor:ComponentDescriptor<IDocumentSession>
    {
        protected override void ApplyToModel(IKernel kernel, ComponentModel model)
        {
            model.Lifecycle.AddFirst(new SaveDocumentSessionChangesConcern());
        }
    }
}