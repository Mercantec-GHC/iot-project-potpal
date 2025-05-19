using Frontend.Repo;
using Microsoft.AspNetCore.Components;
using Models;
using System.Threading.Tasks;

namespace Frontend.Components.Pages
{
    public partial class PlantDetails
    {
        [Parameter]
        public required string PlantId { get; set; }
        PlantRepo plantRepo = new PlantRepo();
        public Plant plantData {  get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) 
            {
                plantData = await plantRepo.PlantData(PlantId);
            }

        }

    }
}
