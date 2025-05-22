using Microsoft.AspNetCore.Components;
using Models;
using Frontend.Repo;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace Frontend.Components.Pages
{
    public partial class Signup : ComponentBase
    {
        public CreateUserDTO userCreate { get; set; } = new CreateUserDTO();

        public UserDTO UserResult { get; set; } = new();

        private UserRepo userRepo = new UserRepo();

        public async Task ConfirmSignUp()
        {
            UserResult = await userRepo.AddAsync(userCreate);
            StateHasChanged();
        }

    }
}
