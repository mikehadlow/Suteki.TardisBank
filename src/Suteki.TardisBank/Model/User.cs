namespace Suteki.TardisBank.Model
{
    public abstract class User
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string UserName { get; private set; }

        protected User(string name, string userName)
        {
            Id = UserIdFromUserName(userName);
            Name = name;
            UserName = userName;
        }

        public static string UserIdFromUserName(string userName)
        {
            return string.Format("users/{0}", userName);
        }
    }
}