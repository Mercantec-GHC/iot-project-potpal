using System;
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

    public async Task<ShopItem> GetByIDAsync(int ID)
    {
        return await _dbContext.ShopItems
            .FirstOrDefaultAsync(s => s.Id == ID);
    }

    public async Task AddAsync(ShopItem shopItem)
    {
        await _dbContext.ShopItems.AddAsync(shopItem);
        await _dbContext.SaveChangesAsync();
    }

    internal async Task<string> AddItemToCartAsync(int itemID, string userToken)
    {
        var user = await _dbContext.Users
        .Include(u => u.ShopItemsInCart) // Load cart
        .FirstOrDefaultAsync(u => u.token == userToken);

        var item = await _dbContext.ShopItems.FindAsync(itemID);

        if (user == null || item == null)
        {
            return "USER_OR_ITEM_NOT_FOUND";
        }

        if (user.ShopItemsInCart == null)
        {
            user.ShopItemsInCart = new List<ShopItem>();
        }

        // Check if item is already in the cart
        if (user.ShopItemsInCart.Any(i => i.Id == itemID))
        {
            return "ITEM_ALREADY_IN_CART";
        }

        user.ShopItemsInCart.Add(item);
        await _dbContext.SaveChangesAsync();

        return "ITEM_ADDED";
    }
}
