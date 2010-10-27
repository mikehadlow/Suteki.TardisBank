using System.Web.Security;

namespace Suteki.TardisBank.Services
{
    public interface IFormsAuthenticationService
    {
        void SignOut();
        void SetAuthCookie(string userName, bool createPersistentCookie);
        string HashAndSalt(string userName, string password);
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

        public string HashAndSalt(string userName, string password)
        {
            var saltedPassword = userName + password;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(saltedPassword, "SHA1");
        }
    }
}