using System.Web.Mvc;

namespace Suteki.TardisBank.Helpers
{
    public static class StatusCode
    {
        public static ActionResult NotFound
        {
            get { return new HttpStatusCodeResult(404); }
        }

        public static ActionResult Ok
        {
            get { return new HttpStatusCodeResult(200); }
        }
    }
}