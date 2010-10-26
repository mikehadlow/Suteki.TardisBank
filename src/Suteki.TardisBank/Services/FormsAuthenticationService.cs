using System.Web.Security;

namespace Suteki.TardisBank.Services
{
    public interface IFormsAuthenticationService
    {
        void SignOut();
        void SetAuthCookie(string userName, bool createPersistentCookie);
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
    }
}