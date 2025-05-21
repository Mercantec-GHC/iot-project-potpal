using Models;

namespace Frontend.Repo
{
    public interface IUserAuth
    {
        User GetUser();
        User SetUser(User userResult);
    }
}