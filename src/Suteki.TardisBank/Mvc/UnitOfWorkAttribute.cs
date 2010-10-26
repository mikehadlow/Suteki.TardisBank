using System.Web.Mvc;
using Castle.Windsor;
using Raven.Client;

namespace Suteki.TardisBank.Mvc
{
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                if (filterContext.HttpContext != null)
                {
                    var container = ((IContainerAccessor) filterContext.HttpContext.ApplicationInstance).Container;
                    var session = container.Resolve<IDocumentSession>();
                    session.SaveChanges();
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}