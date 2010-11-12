using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace Suteki.TardisBank.IoC
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            if (controllerName == null)
            {
                throw new ArgumentNullException("controllerName");
            }

            try
            {
                return kernel.Resolve<IController>(controllerName + "controller");
            }
            catch (ComponentNotFoundException e)
            {
                throw new ApplicationException(string.Format("No controller with name '{0}' found", controllerName), e);
            }
        }

        public override void ReleaseController(IController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            kernel.ReleaseComponent(controller);
        }
    }
}