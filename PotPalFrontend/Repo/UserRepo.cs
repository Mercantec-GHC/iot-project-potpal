using Models;
using PotPalFrontend.Services;

namespace PotPalFrontend.Repo
{
    public class UserRepo
    {
        private readonly UserServices _userServices;

        public UserRepo(UserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<UserDTO?> LoginAsync(UserLoginDTO login)
        {
            Console.WriteLine("Sending POST to api/user/login");
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return null;

            return await _userServices.LoginAsync(login);
        }

        public async Task<UserDTO?> AddAsync(CreateUserDTO signUp)
        {
            if (signUp == null || string.IsNullOrEmpty(signUp.Email) || string.IsNullOrEmpty(signUp.UserName) || string.IsNullOrEmpty(signUp.Password))
                return null;

            return await _userServices.AddAsync(signUp);
        }
    }
}
