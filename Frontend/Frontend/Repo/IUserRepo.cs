using Models;

namespace PotPalFrontend.Repo
{
    public interface IUserRepo
    {
        Task<UserDTO> AddAsync(CreateUserDTO signUp);
        Task<User> LoginAsync(UserLoginDTO login);
    }
}