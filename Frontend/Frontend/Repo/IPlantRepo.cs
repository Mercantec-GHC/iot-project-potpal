using Models;

namespace Frontend.Repo
{
    public interface IPlantRepo
    {
        Task<Plant> PlantData(string plantGuid);
        Task<bool> UpdatePlantInformationAsync(Plant plant);
    }
}