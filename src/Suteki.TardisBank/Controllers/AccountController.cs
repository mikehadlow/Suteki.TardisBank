using System;
using System.Web.Mvc;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Mvc;
using Suteki.TardisBank.Services;
using Suteki.TardisBank.ViewModel;

namespace Suteki.TardisBank.Controllers
{
    public class AccountController : Controller
    {
        readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult MakePayment(string id)
        {
            // id is the child's user name
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            var parent = userService.CurrentUser as Parent;
            var child = userService.GetUserByUserName(id) as Child;

            if (AreNullOrInvalid(parent, child)) return StatusCode.NotFound;

            return View("MakePayment", new MakePaymentViewModel
            {
                ChildId = child.UserName,
                ChildName = child.Name,
                Description = "",
                Amount = 0M
            });
        }

        [HttpPost, UnitOfWork]
        public ActionResult MakePayment(MakePaymentViewModel makePaymentViewModel)
        {
            if (!ModelState.IsValid) return View("MakePayment", makePaymentViewModel);

            if (makePaymentViewModel == null)
            {
                throw new ArgumentNullException("makePaymentViewModel");
            }

            var parent = userService.CurrentUser as Parent;
            var child = userService.GetUserByUserName(makePaymentViewModel.ChildId) as Child;

            if (AreNullOrInvalid(parent, child)) return StatusCode.NotFound;

            parent.MakePaymentTo(child, makePaymentViewModel.Amount, makePaymentViewModel.Description);

            return View("PaymentConfirmation", makePaymentViewModel);
        }

        static bool AreNullOrInvalid(Parent parent, Child child)
        {
            if (parent == null || child == null) return true;

            if (!parent.HasChild(child))
            {
                throw new TardisBankException("'{0}' is not a child of '{1}'", child.UserName, parent.UserName);
            }

            return false;
        }

        [HttpGet]
        public ActionResult ParentView(string id)
        {
            var parent = userService.CurrentUser as Parent;
            var child = userService.GetUserByUserName(id) as Child;

            if (AreNullOrInvalid(parent, child)) return StatusCode.NotFound;

            return View("Summary", child.Account);
        }

        [HttpGet]
        public ActionResult ChildView()
        {
            var child = userService.CurrentUser as Child;
            if (child == null)
            {
                return StatusCode.NotFound;
            }
            return View("Summary", child.Account);
        }

        [HttpGet]
        public ActionResult WithdrawCash()
        {
            var child = userService.CurrentUser as Child;
            if (child == null)
            {
                return StatusCode.NotFound;
            }

            return View("WithdrawCash", new WithdrawCashViewModel
            {
                Amount = 0M,
                Description = ""
            });
        }

        [HttpPost, UnitOfWork]
        public ActionResult WithdrawCash(WithdrawCashViewModel withdrawCashViewModel)
        {
            if (withdrawCashViewModel == null)
            {
                throw new ArgumentNullException("withdrawCashViewModel");
            }

            if (!ModelState.IsValid) return View("WithdrawCash", withdrawCashViewModel);

            var child = userService.CurrentUser as Child;
            if (child == null)
            {
                return StatusCode.NotFound;
            }
            var parent = userService.GetUser(child.ParentId) as Parent;
            if (parent == null)
            {
                throw new TardisBankException("Parent with id '{0}' not found", child.ParentId);
            }

            try
            {
                child.WithdrawCashFromParent(
                    parent, 
                    withdrawCashViewModel.Amount, 
                    withdrawCashViewModel.Description);
            }
            catch (CashWithdrawException cashWithdrawException)
            {
                ModelState.AddModelError("Amount", cashWithdrawException.Message);
                return View("WithdrawCash", withdrawCashViewModel);
            }

            return View("WithdrawCashConfirm", withdrawCashViewModel);
        }
    }
}