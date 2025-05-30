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

    public async Task<Plant> AddAsync(PlantPostDTO newPlant)
    {
        // Check if the user exists
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == newPlant.UserEmail);
        if (user == null)
            throw new Exception("User not found");



        // Create a new Plant entity from the DTO
        Plant createPlant = new Plant
        {
            GUID = newPlant.Guid,
            PlantName = newPlant.PlantName,
            IdealSoilMoisture = newPlant.IdealSoilMoisture,
            IdealTemperature = newPlant.IdealTemperature,
            IdealLightLevel = newPlant.IdealLightLevel,
            IdealAirHumidity = newPlant.IdealAirHumidity,
            UserEmail = newPlant.UserEmail
        };

        _context.Plants.Add(createPlant);
        await _context.SaveChangesAsync();
        return createPlant;
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
