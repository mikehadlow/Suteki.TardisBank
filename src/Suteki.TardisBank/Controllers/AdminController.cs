using System.Web.Mvc;
using Suteki.TardisBank.Helpers;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Mvc;
using Suteki.TardisBank.Services;
using Suteki.TardisBank.ViewModel;

namespace Suteki.TardisBank.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        readonly IUserService userService;
        readonly IFormsAuthenticationService formsAuthenticationService;

        public AdminController(IUserService userService, IFormsAuthenticationService formsAuthenticationService)
        {
            this.userService = userService;
            this.formsAuthenticationService = formsAuthenticationService;
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

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            
            var oldHashedPassword = GetHashedPassword(model.OldPassword);
            bool passwordIsOk = false;
            if (oldHashedPassword == userService.CurrentUser.Password)
            {
                passwordIsOk = true;
            }
            else
            {
                ModelState.AddModelError("OldPassword", "The password you provided is invalid");
            }
            if(ModelState.IsValid && passwordIsOk)
            {
                var newHashedPassword = GetHashedPassword(model.NewPassword);
                userService.CurrentUser.ResetPassword(newHashedPassword);
                // TODO: we should have also a flash message saying it's been successful
                return RedirectToAction("Index");
            }
            return View();
        }

        private string GetHashedPassword(string password)
        {
            return formsAuthenticationService.HashAndSalt(
                userService.CurrentUser.UserName,
                password);
        }
    }
}