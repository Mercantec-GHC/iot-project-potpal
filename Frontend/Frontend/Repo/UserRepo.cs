using Models;
using Frontend.Services;
using Frontend.Repo;
using Models;
using PotPalFrontend.Services;

namespace Frontend.Repo
{
    public class UserRepo : IUserRepo
    {
        IUserAuth userAuth;
        public UserRepo(IUserAuth userAuth)
        {
            this.userAuth = userAuth;
        }


        UserServices userServices = new UserServices();

        public async Task<User> LoginAsync(UserLoginDTO login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password)) return null;

            //Stores the login data in userDTO
            UserDTO userDTO = await userServices.LoginAsync(login);
            User tempUser;
            if (userDTO != null)
            {
                tempUser = new User
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    Token = userDTO.Token,
                };
                //Saves the token in the local storage
                userAuth.SetUser(tempUser);
            }
            else
            {
                //If the login fails, remove the token from local storage
                return null;
            }

            return tempUser;
        }

        public async Task<UserDTO> AddAsync(CreateUserDTO signUp)
        {
            if (signUp == null || string.IsNullOrEmpty(signUp.Email) || string.IsNullOrEmpty(signUp.UserName) || string.IsNullOrEmpty(signUp.Password)) return null;


            UserDTO userDTO = await userServices.AddAsync(signUp);

            return userDTO != null ? userDTO : null;
        }




    }
}
