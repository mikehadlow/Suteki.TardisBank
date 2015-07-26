using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Suteki.TardisBank.Mvc
{
    public class CultureActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if(request == null || request.UserLanguages == null)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var culture = new CultureInfo(request.UserLanguages[0]);
            Thread.CurrentThread.CurrentCulture = culture;

            base.OnActionExecuting(filterContext);
            
        }

       
    }
}