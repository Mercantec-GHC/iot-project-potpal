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

    internal async Task<string> AddItemToCartAsync(CartItem cart)
    {
        var user = await _dbContext.Users.FindAsync(cart.UserToken);
        var item = await _dbContext.ShopItems.FindAsync(cart.ItemId);

        if (user == null || item == null)
        {
            return "USER_OR_ITEM_NOT_FOUND";
        }

        var existingCartItem = await _dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.UserToken == cart.UserToken && ci.ItemId == cart.ItemId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += 1;
        }

        var cartItem = new CartItem
        {
            UserToken = cart.UserToken,
            ItemId = cart.ItemId,
            Quantity = 1
        };

        await _dbContext.CartItems.AddAsync(cartItem);
        await _dbContext.SaveChangesAsync();

        return "ITEM_ADDED";
    }

}
