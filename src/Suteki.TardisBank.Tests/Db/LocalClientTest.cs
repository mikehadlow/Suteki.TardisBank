using System.IO;
using System.Reflection;
using Raven.Client.Document;
using Raven.Database;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Tests.Db
{
    public abstract class LocalClientTest
    {
        private string path;

        protected DocumentStore NewDocumentStore()
        {
            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = Path.Combine(path, "TestDb").Substring(6);

            if (Directory.Exists(path))
                Directory.Delete(path, true);

            var documentStore = new DocumentStore
            {
                Configuration = new RavenConfiguration
                {
                    DataDirectory = path,
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true
                },
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