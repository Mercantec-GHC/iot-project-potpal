using Microsoft.AspNetCore.Components;
using Models;
using Frontend.Repo;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace Frontend.Components.Pages
{
    public partial class Login : ComponentBase
    {
        public UserLoginDTO userLogin { get; set; } = new UserLoginDTO();

        public UserDTO UserResult { get; set; } = new();

        private UserRepo userRepo = new UserRepo(); 

        public async Task ConfirmLogin()
        {
            UserResult = await userRepo.LoginAsync(userLogin);
            StateHasChanged();
        }
    }
}
