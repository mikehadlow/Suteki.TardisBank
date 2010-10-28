using Raven.Client.Document;
using Raven.Client.Client;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Tests.Db
{
    public abstract class LocalClientTest
    {
        protected DocumentStore NewDocumentStore()
        {
            var documentStore = new EmbeddablDocumentStore()
            {
                RunInMemory = true,
                Conventions =
                {
                    FindTypeTagName = type => typeof(User).IsAssignableFrom(type) ? "users" : null
                }
            };
            documentStore.Initialize();
            return documentStore;
        }
    }
}