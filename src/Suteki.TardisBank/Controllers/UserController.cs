using System.Web.Mvc;
using Suteki.TardisBank.Services;
using Suteki.TardisBank.ViewModel;

namespace Suteki.TardisBank.Controllers
{
    public class UserController : Controller
    {
        readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            var user = userService.CurrentUser;
            var name = user == null ? "Guest" : user.Name;
            return View("Index", new UserViewModel { Name = name });
        }
    }
}