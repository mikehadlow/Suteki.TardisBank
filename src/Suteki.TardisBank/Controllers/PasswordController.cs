using System;
using System.Web.Mvc;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Mvc;
using Suteki.TardisBank.Services;
using Suteki.TardisBank.ViewModel;

namespace Suteki.TardisBank.Controllers
{
    public class PasswordController : Controller
    {
        readonly IUserService userService;
        readonly IEmailService emailService;
        readonly IFormsAuthenticationService formsAuthenticationService;

        public PasswordController(IUserService userService, IEmailService emailService, IFormsAuthenticationService formsAuthenticationService)
        {
            this.userService = userService;
            this.formsAuthenticationService = formsAuthenticationService;
            this.emailService = emailService;
        }

        [HttpGet]
        public ActionResult Forgot()
        {
            return View("Forgot", new ForgottenPasswordViewModel
            {
                UserName = "",
            });
        }

        [HttpPost, UnitOfWork]
        public ActionResult Forgot(ForgottenPasswordViewModel forgottenPasswordViewModel)
        {
            if (!ModelState.IsValid) return View("Forgot", forgottenPasswordViewModel);
            if (forgottenPasswordViewModel == null)
            {
                throw new ArgumentNullException("forgottenPasswordViewModel");
            }

            var user = userService.GetUserByUserName(forgottenPasswordViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError("UserName", "We don't have a record of that email or user name.");
                return View("Forgot", forgottenPasswordViewModel);
            }

            if (user is Child)
            {
                var parent = userService.GetUser(((Child)user).ParentId);
                if (parent == null)
                {
                    throw new TardisBankException("Missing parent: {0}", ((Child)user).ParentId);
                }

                SendPasswordResetEmail(user, toAddress: parent.UserName);
                return RedirectToAction("ChildConfirm");
            }

            if (user is Parent)
            {
                SendPasswordResetEmail(user, toAddress: user.UserName);
                return RedirectToAction("ParentConfirm");
            }

            throw new TardisBankException("unknown User subtype");
        }

        string GetNewPasswordFor(User user)
        {
            var newPassword = Guid.NewGuid().ToString().Substring(0, 5);
            var hashedPassword = formsAuthenticationService.HashAndSalt(user.UserName, newPassword);
            user.ResetPassword(hashedPassword);
            return newPassword;
        }

        void SendPasswordResetEmail(User user, string toAddress)
        {
            var isChildString = user is Child ? user.Name + "'s" : "Your";
            var subject = string.Format("Tardis Bank: {0} reset password", isChildString);
            var body = string.Format("Here is {0} new password: {1}", isChildString, GetNewPasswordFor(user));
            emailService.SendEmail(toAddress, subject, body);
        }

        [HttpGet]
        public ViewResult ChildConfirm()
        {
            return View();
        }

        [HttpGet]
        public ViewResult ParentConfirm()
        {
            return View();
        }
    }
}