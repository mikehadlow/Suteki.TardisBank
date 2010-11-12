using Castle.Core;
using Raven.Client;

namespace Suteki.TardisBank.IoC
{
    public class SaveDocumentSessionChangesConcern : IDecommissionConcern
    {
        public void Apply(ComponentModel model, object component)
        {
            var session = ((IDocumentSession) component);
            session.SaveChanges();
        }
    }
}