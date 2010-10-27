using System.Web.Mvc;

namespace Suteki.TardisBank.Controllers
{
    public static class StatusCode
    {
        public static ActionResult NotFound
        {
            get { return new HttpStatusCodeResult(404); }
        }
    }
}