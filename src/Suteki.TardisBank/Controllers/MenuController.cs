using System.Web.Mvc;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Services;

namespace Suteki.TardisBank.Controllers
{
    public class MenuController : Controller
    {
        readonly IUserService userService;

        public MenuController(IUserService userService)
        {
            this.userService = userService;
        }

        [ChildActionOnly]
        public ViewResult Index()
        {
            var user = userService.CurrentUser;

            if (user == null) return View("GuestMenu");
            if (user is Parent) return View("ParentMenu", user as Parent);
            if (user is Child) return View("ChildMenu", user as Child);

            throw new TardisBankException("Unknown User type");
        }
    }
}