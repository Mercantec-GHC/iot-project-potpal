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
        await _dbContext.Metrics.AddAsync(metric);
        await _dbContext.SaveChangesAsync();
    }
}