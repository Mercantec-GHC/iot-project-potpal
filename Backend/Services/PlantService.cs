using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class PlantService(PlantRepo plantRepo)
{
  


    private readonly PlantRepo _plantRepo = plantRepo;


    public async Task<Plant?> GetByIdAsync(int id)
    {
        return await _plantRepo.GetByIdAsync(id);
    }


    public async Task<IEnumerable<Plant>> GetAllAsync()
    {
        return await _plantRepo.GetAllAsync();
    }


    public async Task AddAsync(Plant plant)
    {
        await _plantRepo.AddAsync(plant);
    }
   
    
}