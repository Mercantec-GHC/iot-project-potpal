using Backend.Models;
using Models;

public class PlantService(PlantRepo plantRepo)
{
    private readonly PlantRepo _plantRepo = plantRepo;

    public async Task<Plant?> GetByGuidAsync(string guid, string email)
    {
        return await _plantRepo.GetByGuidAsync(guid, email);
    }
    public async Task<IEnumerable<Plant>> GetByUserAsync(string email)
    {
        return await _plantRepo.GetByUserAsync(email);
    }

    public async Task<IEnumerable<Plant>> GetAllAsync()
    {
        return await _plantRepo.GetAllAsync();
    }

    public async Task AddAsync(PlantPostDTO plantDTO)
    {
        await _plantRepo.AddAsync(plantDTO);
    }
   
    public async Task UpdateAsync(PlantDTO plant)
    {
        Plant EntityPlant = new Plant
        {
            GUID = plant.GUID,
            PlantName = plant.PlantName,
            IdealSoilMoisture = plant.IdealSoilMoisture,
            IdealTemperature = plant.IdealTemperature,
            IdealLightLevel = plant.IdealLightLevel,
            IdealAirHumidity = plant.IdealAirHumidity,
            UserEmail = plant.UserEmail,
        };

        await _plantRepo.UpdateAsync(EntityPlant);
    }
}