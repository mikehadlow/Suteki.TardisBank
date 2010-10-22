using System;
using System.Web.Mvc;

namespace Suteki.TardisBank.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View("Index");
        }
    }
}