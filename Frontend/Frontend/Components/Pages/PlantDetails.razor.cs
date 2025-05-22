using Frontend.Repo;
using Microsoft.AspNetCore.Components;
using Models;
using System.Threading.Tasks;

namespace Frontend.Components.Pages
{
    public partial class PlantDetails
    {
        [Inject]
        IUserAuth userAuth { get; set; }


        [Parameter]
        public required string PlantId { get; set; }
        PlantRepo plantRepo = new PlantRepo();
        public Plant plantData {  get; set; }

        public User UserResult { get; set; } = new User();


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender) 
            {
                UserResult = userAuth.GetUser();
                plantData = await plantRepo.PlantData(PlantId);
                StateHasChanged();
            }
            
        }



    }
}
