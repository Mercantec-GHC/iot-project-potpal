<<<<<<< Updated upstream:PotPalFrontend/Components/Pages/Login.razor.cs
﻿using Models;
using PotPalFrontend.Repo;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace PotPalFrontend.Components.Pages
{
    public partial class Login
    {
        public UserLoginDTO userLogin { get; set; } = new UserLoginDTO();

        public UserDTO UserResult { get; set; }
=======
﻿// using Microsoft.AspNetCore.Components;
// using Models;
// using PotPalFrontend.Repo;
// using System.Reflection.Metadata.Ecma335;
// using System.Security.Principal;

// namespace Frontend.Components.Pages
// {
//     public partial class Login : ComponentBase
//     {
//         public UserLoginDTO userLogin { get; set; } = new UserLoginDTO();

//         public UserDTO UserResult { get; set; } = new();
>>>>>>> Stashed changes:Frontend/Frontend/Components/Pages/Login.razor.cs

//         private UserRepo userRepo = new UserRepo(); 

//         public async Task ConfirmLogin()
//         {
//             Console.WriteLine("Login button clicked");
//             UserResult = await userRepo.LoginAsync(userLogin);
//             StateHasChanged();
//         }

//     }
// }
