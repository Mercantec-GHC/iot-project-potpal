using System;
using Microsoft.EntityFrameworkCore;
using Models;
using Database;

public class PlantRepo
{
    private readonly PotPalDbContext _context;

    public PlantRepo(PotPalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Plant>> GetAllAsync()
    {
        return await _context.Plants.Include(p => p.User).Include(p => p.Metrics).ToListAsync();
    }

    public async Task<Plant?> GetByGuidAsync(string guid)
    {
        return await _context.Plants
            .Include(p => p.User)
            .Include(p => p.Metrics)
            .FirstOrDefaultAsync(p => p.GUID == guid);
    }

    public async Task<List<Plant>> GetByUserAsync(string email)
    {
        return await _context.Plants
            .Include(p => p.User)
            .Include(p => p.Metrics)
            .Where(p => p.UserEmail == email)
            .ToListAsync();
    }

    public async Task<Plant> AddAsync(Plant newPlant)
    {
        // Prevent EF from inserting the User entity again
        _context.Entry(newPlant.User).State = EntityState.Unchanged;

        _context.Plants.Add(newPlant);
        await _context.SaveChangesAsync();
        return newPlant;
    }

    public async Task<Plant> UpdateAsync(Plant updatedPlant)
    {
        var existing = await _context.Plants.FindAsync(updatedPlant.GUID);
        if (existing == null)
        {
            throw new Exception("Plant not found");
        }

        // Update scalar fields
        existing.PlantName = updatedPlant.PlantName;
        existing.IdealSoilMoisture = updatedPlant.IdealSoilMoisture;
        existing.IdealTemperature = updatedPlant.IdealTemperature;
        existing.IdealLightLevel = updatedPlant.IdealLightLevel;
        existing.IdealAirHumidity = updatedPlant.IdealAirHumidity;

        // Update the foreign key
        existing.UserEmail = updatedPlant.UserEmail;

        // Prevent EF from trying to insert/update the User entity
        _context.Entry(existing.User).State = EntityState.Unchanged;

        await _context.SaveChangesAsync();
        return existing;
    }
}
