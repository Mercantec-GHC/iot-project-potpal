using System;
using Database;
using Microsoft.AspNetCore.Http.HttpResults;
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
        var user = await _dbContext.Users.FindAsync(cart.UserEmail);
        var item = await _dbContext.ShopItems.FindAsync(cart.ItemId);

        if (user == null || item == null)
        {
            return "USER_OR_ITEM_NOT_FOUND";
        }

        var existingCartItem = await _dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.UserEmail == cart.UserEmail && ci.ItemId == cart.ItemId);

        if (existingCartItem != null)
        {
            return "ITEM_ALREADY_IN_CART";
        }

        var cartItem = new CartItem
        {
            UserEmail = cart.UserEmail,
            ItemId = cart.ItemId,
            Quantity = 1
        };

        await _dbContext.CartItems.AddAsync(cartItem);
        await _dbContext.SaveChangesAsync();

        return "ITEM_ADDED";
    }

    public async Task<IEnumerable<CartItem>> GetCartByUserAsync(string userEmail)
    {

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

        if(user == null)
        {
            throw new KeyNotFoundException("User not found");
        }
        return await _dbContext.CartItems
            .Include(ci => ci.User)
            .Include(ci => ci.ShopItem)
            .Where(ci => ci.User.Email == userEmail)
            .ToListAsync();
    }
}
