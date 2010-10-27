using System.Web.Mvc;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Services;

namespace Suteki.TardisBank.Controllers
{
    public class ChildController : Controller
    {
        readonly IUserService userService;

        public ChildController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var parent = userService.CurrentUser as Parent;
            if (parent == null)
            {
                return StatusCode.NotFound;
            }

            return View("Index", parent);
        }
    }
}