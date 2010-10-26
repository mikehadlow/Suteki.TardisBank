using System;
using System.Web.Mvc;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Mvc;
using Suteki.TardisBank.Services;
using Suteki.TardisBank.ViewModel;

namespace Suteki.TardisBank.Controllers
{
    public class UserController : Controller
    {
        readonly IUserService userService;
        readonly IFormsAuthenticationService formsAuthenticationService;

        public UserController(IUserService userService, IFormsAuthenticationService formsAuthenticationService)
        {
            this.userService = userService;
            this.formsAuthenticationService = formsAuthenticationService;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            var user = userService.CurrentUser;
            var name = user == null ? "Guest" : user.Name;
            return View("Index", new UserViewModel { Name = name, IsLoggedIn = user != null });
        }

        [HttpGet]
        public ActionResult Register()
        {
            var registrationViewModel = new RegistrationViewModel
            {
                Email = "",
                Name = "",
                Password = ""
            };
            return View("Register", registrationViewModel);
        }

        [HttpPost, UnitOfWork]
        public ActionResult Register(RegistrationViewModel registrationViewModel)
        {
            if (registrationViewModel == null)
            {
                throw new ArgumentNullException("registrationViewModel");
            }

            if (ModelState.IsValid)
            {
                var parent = new Parent(registrationViewModel.Name, registrationViewModel.Email);
                userService.SaveUser(parent);
                formsAuthenticationService.SetAuthCookie(parent.UserName, false);
                return RedirectToAction("Confirm");
            }

            return View("Register", registrationViewModel);
        }

        [HttpGet]
        public ActionResult Confirm()
        {
            return View("Confirm");
        }

        [HttpGet]
        public ActionResult Login()
        {
            var loginViewModel = new LoginViewModel
            {
                Name = "",
                Password = ""
            };
            return View("Login", loginViewModel);
        }

        [HttpPost, UnitOfWork]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel == null)
            {
                throw new ArgumentNullException("loginViewModel");
            }

            if (ModelState.IsValid)
            {
                var parent = userService.GetUserByUserName(loginViewModel.Name);
                if (parent != null)
                {
                    formsAuthenticationService.SetAuthCookie(parent.UserName, false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Name", "Invalid Name");
            }

            return View("Login", loginViewModel);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            formsAuthenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}