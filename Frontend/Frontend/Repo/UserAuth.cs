using Models;

namespace Frontend.Repo
{
    public class UserAuth : IUserAuth
    {
        private User user;

        public User GetUser()
        {
            return user;
        }

        public User SetUser(User userResult)
        {
            user = userResult;
            return user;
        }

    }
}
