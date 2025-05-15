using Microsoft.AspNetCore.Components;
using Models;
using PotPalFrontend.Repo;

namespace Frontend.Components.Pages
{
    public partial class Signup : ComponentBase
    {
        [Inject]
        public UserRepo userRepo { get; set; }

        public CreateUserDTO userCreate { get; set; } = new CreateUserDTO();

        public UserDTO UserResult { get; set; } = new();

        public async Task ConfirmSignUp()
        {
            UserResult = await userRepo.AddAsync(userCreate);
            StateHasChanged();
        }
    }
}
