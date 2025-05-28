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

    public async Task<Plant?> GetPlantWithUserAsync(string plantGuid)
    {
        return await _dbContext.Plants
                        .Include(p => p.User)
                        .FirstOrDefaultAsync(p => p.GUID == plantGuid);
    }

    public async Task<Metric?> GetByPlantGUIDAsync(string guid)
    {
        return await _dbContext.Metrics
                    .FirstOrDefaultAsync(m => m.PlantGUID == guid);
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

    public async Task UpdateAsync(Metric metric)
    {
        _dbContext.Metrics.Update(metric);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(Metric metric)
    {
        _dbContext.Metrics.Remove(metric);
        await _dbContext.SaveChangesAsync();
    }
}
