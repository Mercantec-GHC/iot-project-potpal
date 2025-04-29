using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class PlantService(PlantRepo plantRepo)
{
  


    private readonly PlantRepo _plantRepo = plantRepo;


    public async Task<Plant?> GetByGuidAsync(string guid)
    {
        return await _plantRepo.GetByGuidAsync(guid);
    }
    public async Task<IEnumerable<Plant>> GetByUserAsync(string email)
    {
        return await _plantRepo.GetByUserAsync(email);
    }


    public async Task<IEnumerable<Plant>> GetAllAsync()
    {
        return await _plantRepo.GetAllAsync();
    }


    public async Task AddAsync(Plant plant)
    {
        await _plantRepo.AddAsync(plant);
    }
   
    public async Task UpdateAsync(Plant plant)
    {
        await _plantRepo.UpdateAsync(plant);
    }
}