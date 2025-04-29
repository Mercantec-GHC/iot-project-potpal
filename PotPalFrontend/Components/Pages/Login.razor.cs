using Models;
using PotPalFrontend.Repo;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace PotPalFrontend.Components.Pages
{
    public partial class Login
    {
        public UserLoginDTO userLogin { get; set; } = new UserLoginDTO();

        public UserDTO UserResult { get; set; }

        private UserRepo userRepo = new UserRepo(); 

        public async Task ConfirmLogin()
        {
            UserResult = await userRepo.LoginAsync(userLogin);
            StateHasChanged();
        }

    }
}
