using System.Web.Mvc;
using DotNetOpenAuth.OpenId.RelyingParty;
using Suteki.TardisBank.Model;
using Suteki.TardisBank.Mvc;
using Suteki.TardisBank.Services;

namespace Suteki.TardisBank.Controllers
{
    public class OpenidController : Controller
    {
        readonly IFormsAuthenticationService formsAuthenticationService;
        readonly IOpenIdService openIdService;
        readonly IUserService userService;

        public OpenidController(
            IFormsAuthenticationService formsAuthenticationService, 
            IOpenIdService openIdService, 
            IUserService userService)
        {
            this.formsAuthenticationService = formsAuthenticationService;
            this.userService = userService;
            this.openIdService = openIdService;
        }


        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", new {ReturnUrl = "Index"});
            }

            return View("Index");
        }

        public ActionResult Logout()
        {
            formsAuthenticationService.SignOut();
            return Redirect("~/Home");
        }

        public ActionResult Login()
        {
            // Stage 1: display login form to user
            return View("Login");
        }

        [ValidateInput(false), UnitOfWork]
        public ActionResult Authenticate(string returnUrl)
        {
            var response = openIdService.GetResponse();

            // Stage 2: Make the request to the openId provider
            if (response == null)
            {
                try
                {
                    return openIdService.CreateRequest(Request.Form["openid_identifier"]);
                }
                catch (OpenIdException openIdException)
                {
                    ViewData["Message"] = openIdException.Message;
                    return View("Login");
                }
            }
            
            // Stage 3: OpenID Provider sending assertion response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    formsAuthenticationService.SetAuthCookie(response.ClaimedIdentifier, false);

                    CreateNewParentIfTheyDontAlreadyExist(response);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                case AuthenticationStatus.Canceled:
                    ViewData["Message"] = "Canceled at provider";
                    return View("Login");
                case AuthenticationStatus.Failed:
                    ViewData["Message"] = response.Exception.Message;
                    return View("Login");
            }

            throw new TardisBankException("Unknown AuthenticationStatus Response");
        }

        void CreateNewParentIfTheyDontAlreadyExist(IAuthenticationResponse response)
        {
            var userId = Model.User.UserIdFromUserName(response.ClaimedIdentifier);
            if (userService.GetUser(userId) != null) return;

            var parent = new Parent(response.FriendlyIdentifierForDisplay, response.ClaimedIdentifier, "todo");
            userService.SaveUser(parent);
        }
    }
}