using System.Web.Mvc;

namespace Suteki.TardisBank.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View("Index");
        }

        public ViewResult Error()
        {
            // throw an error for testing
            throw new TardisBankException("Something really bad happened!");
        }

        public ActionResult NotFound()
        {
            return new HttpStatusCodeResult(404);
        }

        public ViewResult About()
        {
            return View();
        }

        public ViewResult Legal()
        {
            return View();
        }
    }
}