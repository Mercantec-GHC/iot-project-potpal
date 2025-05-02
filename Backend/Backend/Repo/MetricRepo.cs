using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class MetricRepo
{
    private readonly PotPalDbContext _dbContext;

    public MetricRepo(PotPalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Metric>> GetByPlantAsync(string guid)
    {
        return await _dbContext.Metrics
            .Where(m => m.PlantGUID == guid)
            .ToListAsync();
    }

    public async Task<IEnumerable<Metric>> GetAllAsync()
    {
        return await _dbContext.Metrics.ToListAsync();
    }

    public async Task AddAsync(Metric metric)
    {
        // Retrieve the plant by GUID
        var plant = await _dbContext.Plants.Include(p => p.User).FirstOrDefaultAsync(p => p.GUID == metric.PlantGUID);
        if (plant == null)
        {
            throw new Exception($"Plant with GUID '{metric.PlantGUID}' not found.");
        }

        var user = plant.User;
        if (user == null)
        {
            throw new Exception($"User with email '{plant.UserEmail}' not found.");
        }

        _dbContext.Entry(user).State = EntityState.Unchanged;

        metric.Plant = plant;
        metric.Plant.User = user;

        metric.Timestamp = DateTime.UtcNow;
        await _dbContext.Metrics.AddAsync(metric);
        await _dbContext.SaveChangesAsync();
    }
}