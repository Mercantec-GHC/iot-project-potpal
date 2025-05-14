using Models;
using PotPalFrontend.Repo;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace PotPalFrontend.Components.Pages
{
    public partial class Signup
    {
        public CreateUserDTO userCreate { get; set; } = new CreateUserDTO();

        public UserDTO UserResult { get; set; }

        [Inject]
        public UserRepo userRepo { get; set; }


        public async Task ConfirmSignUp()
        {
            UserResult = await userRepo.AddAsync(userCreate);
            StateHasChanged();
        }

    }
}
