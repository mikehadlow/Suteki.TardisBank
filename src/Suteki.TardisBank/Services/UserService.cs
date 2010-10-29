using System;
using System.Linq;
using System.Collections.Generic;
using Raven.Client;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Services
{
    public interface IUserService
    {
        User CurrentUser { get; }
        User GetUser(string userId);
        User GetUserByUserName(string userName);
        User GetUserByActivationKey(string activationKey);
        void SaveUser(User user);
        IEnumerable<Child> GetChildrenOf(Parent parent);

        bool AreNullOrNotRelated(Parent parent, Child child);
        bool IsNotChildOfCurrentUser(Child child);
    }

    public class UserService : IUserService
    {
        readonly IDocumentSession session;
        readonly IHttpContextService context;

        public UserService(IDocumentSession session, IHttpContextService context)
        {
            this.session = session;
            this.context = context;
        }

        public User CurrentUser
        {
            get 
            {
                if (!context.UserIsAuthenticated) return null;
                var userId = string.Format("users/{0}", context.UserName);
                return session.Load<User>(userId);
            }
        }

        public User GetUser(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            return session.Load<User>(userId);
        }

        public User GetUserByUserName(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            return session.Load<User>(User.UserIdFromUserName(userName));
        }

        public User GetUserByActivationKey(string activationKey)
        {
            if (activationKey == null)
            {
                throw new ArgumentNullException("activationKey");
            }

            return session.Query<Parent>().Where(x => x.ActivationKey == activationKey).SingleOrDefault();
        }

        public void SaveUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            session.Store(user);
        }

        public IEnumerable<Child> GetChildrenOf(Parent parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            var childIds = parent.Children.Select(x => x.ChildId).ToArray();
            return session.Load<Child>(childIds).AsEnumerable();
        }

        public bool AreNullOrNotRelated(Parent parent, Child child)
        {
            if (parent == null || child == null) return true;

            if (!parent.HasChild(child))
            {
                throw new TardisBankException("'{0}' is not a child of '{1}'", child.UserName, parent.UserName);
            }

            return false;
        }

        public bool IsNotChildOfCurrentUser(Child child)
        {
            var parent = CurrentUser as Parent;
            return (child == null) || (parent == null) || (!parent.HasChild(child));
        }
    }
}