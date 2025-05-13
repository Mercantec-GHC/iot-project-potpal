using Models;
using Frontend.Services;

namespace Frontend.Repo
{
    public class UserRepo
    {
      

        UserServices userServices = new UserServices();

        public async Task<UserDTO> LoginAsync(UserLoginDTO login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password)) return null; 

            //Stores the login data in userDTO
            UserDTO userDTO = await userServices.LoginAsync(login);

            return userDTO != null ? userDTO : null;
        }

        public async Task<UserDTO> AddAsync(CreateUserDTO signUp)
        {
            if (signUp == null || string.IsNullOrEmpty(signUp.Email) || string.IsNullOrEmpty(signUp.UserName) || string.IsNullOrEmpty(signUp.Password)) return null;


            UserDTO userDTO = await userServices.AddAsync(signUp);

            return userDTO != null ? userDTO : null;
        }

        
    }
}
