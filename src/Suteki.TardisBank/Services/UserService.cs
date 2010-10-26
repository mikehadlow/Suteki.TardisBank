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
        void SaveUser(User user);
        IEnumerable<Child> GetChildrenOf(Parent parent);
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
    }
}