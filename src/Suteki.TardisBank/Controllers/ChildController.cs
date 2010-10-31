using System.Web.Mvc;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Mvc;
using Suteki.TardisBank.Services;
using Suteki.TardisBank.ViewModel;

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

            return View(parent);
        }

        [HttpGet]
        public ActionResult DeleteChild(string id)
        {
            // id is the child's user name
            var child = userService.GetUserByUserName(id) as Child;
            if (child == null)
            {
                return StatusCode.NotFound;
            }

            return View(new DeleteChildConfirmViewModel
            {
                ChildId = child.Id,
                ChildName = child.Name
            });
        }

        [HttpPost, UnitOfWork]
        public ActionResult DeleteChild(DeleteChildConfirmViewModel deleteChildConfirmViewModel)
        {
            var parent = userService.CurrentUser as Parent;
            if (parent == null || !parent.HasChild(deleteChildConfirmViewModel.ChildId))
            {
                return StatusCode.NotFound;
            }
            parent.RemoveChild(deleteChildConfirmViewModel.ChildId);
            userService.DeleteUser(deleteChildConfirmViewModel.ChildId);
            return RedirectToAction("Index");
        }
    }
}