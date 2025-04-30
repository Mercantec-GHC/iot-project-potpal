using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class MetricRepo
{
    private readonly PotPalDbContext _context;

    public MetricRepo(PotPalDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Metric metric)
    {
        _context.Metrics.Add(metric);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Metric>> GetByPlantGuidAsync(string plantGuid)
    {
        return await _context.Metrics
            .Where(m => m.PlantGUID == plantGuid)
            .OrderByDescending(m => m.Timestamp)
            .ToListAsync();
    }
}
