using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class ShopRepo
{
    private readonly PotPalDbContext _dbContext;

    public ShopRepo(PotPalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ShopItem>> GetAllAsync()
    {
        return await _dbContext.ShopItems.ToListAsync();
    }

    public async Task AddAsync(ShopItem shopItem)
    {
        await _dbContext.ShopItems.AddAsync(shopItem);
        await _dbContext.SaveChangesAsync();
    }
}
