using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class PlantRepo
{
    private readonly PotPalDbContext _dbContext;

    public PlantRepo(PotPalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Plant?> GetByGuidAsync(string guid)
    {
        return await _dbContext.Plants.FindAsync(guid);
    }

    public async Task<IEnumerable<Plant>> GetAllAsync()
    {
        return await _dbContext.Plants.ToListAsync();
    }

    public async Task AddAsync(Plant plant)
    {
        await _dbContext.Plants.AddAsync(plant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Plant plant)
    {
        _dbContext.Plants.Update(plant);
        await _dbContext.SaveChangesAsync();
    }
}