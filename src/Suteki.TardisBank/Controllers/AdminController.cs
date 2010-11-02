using System.Web.Mvc;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Services;

namespace Suteki.TardisBank.Controllers
{
    public class AdminController : Controller
    {
        readonly IUserService userService;

        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DeleteParentConfirm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DeleteParent()
        {
            var parent = userService.CurrentUser as Parent;
            if (parent == null)
            {
                return StatusCode.NotFound;
            }

            foreach (var childProxy in parent.Children)
            {
                userService.DeleteUser(childProxy.ChildId);
            }
            userService.DeleteUser(parent.Id);

            return RedirectToAction("Logout", "User");
        }
    }
}