using Frontend.Services;
using Models;

namespace Frontend.Repo
{
    public class PlantRepo : IPlantRepo
    {
        PlantServices plantService = new PlantServices();
        IUserAuth userAuth;
        public PlantRepo(IUserAuth userAuth)
        {
            this.userAuth = userAuth;
        }

        public async Task<Plant> PlantData(string plantGuid)
        {
            if (string.IsNullOrEmpty(plantGuid)) return null;

            string token = userAuth.GetToken();
            if (string.IsNullOrEmpty(token)) return null;

            return await plantService.PlantData(plantGuid, token);

        }

        public async Task<bool> UpdatePlantInformationAsync(Plant plant)
        {
            if(plant == null) return false;

            string token = userAuth.GetToken();
            if (string.IsNullOrEmpty(token)) return false;

            return await plantService.UpdatePlantInformationAsync(plant, token);
        }
    }
}
