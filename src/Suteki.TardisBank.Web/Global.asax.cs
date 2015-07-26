using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Suteki.TardisBank.Mvc;

namespace Suteki.TardisBank.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication, IContainerAccessor
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CultureActionFilter());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("robots.txt");
            routes.IgnoreRoute("Content/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "WithRavenId", // Route name
                "{controller}/{action}/{entity}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", entity = UrlParameter.Optional, id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            InitialiseContainer();
            InitialiseControllerFactory();
        }

        protected void InitialiseContainer()
        {
            if (container == null)
            {
                container = new WindsorContainer()
                    .Install(
                        Configuration.FromXmlFile("Windsor.xml"),
                        FromAssembly.InDirectory(new AssemblyFilter(HttpRuntime.BinDirectory, "Suteki.*.dll")));
            }
        }

        protected void InitialiseControllerFactory()
        {
            var controllerFactory = container.Resolve<IControllerFactory>();
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        static IWindsorContainer container;
        public IWindsorContainer Container
        {
            get { return container; }
        }
    }
}