using Microsoft.AspNetCore.Components;
using Models;
using Frontend.Repo;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace Frontend.Components.Pages
{

    public partial class Login : ComponentBase
    {
        [Inject]
        public IUserRepo userRepo { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public UserLoginDTO userLogin { get; set; } = new UserLoginDTO();


        public async Task ConfirmLogin()
        {
            var success = await userRepo.LoginAsync(userLogin);
            StateHasChanged();
        }
    }
}
