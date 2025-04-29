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
        return await _dbContext.Plants
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.GUID == guid);
    }

    public async Task<IEnumerable<Plant>> GetByUserAsync(string email)
    {
        return await _dbContext.Plants
            .Include(p => p.User)
            .Where(p => p.UserEmail == email)
            .ToListAsync();
    }

    public async Task<IEnumerable<Plant>> GetAllAsync()
    {
        return await _dbContext.Plants
            .Include(p => p.User)
            .ToListAsync();
    }

    public async Task<Plant> AddAsync(Plant plant)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == plant.UserEmail);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        plant.User = user;

        _dbContext.Plants.Add(plant);
        await _dbContext.SaveChangesAsync();

        return plant;
    }


    public async Task UpdateAsync(Plant plant)
    {
        _dbContext.Plants.Update(plant);
        await _dbContext.SaveChangesAsync();
    }
}