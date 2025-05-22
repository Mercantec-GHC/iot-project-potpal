using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class ShopService(ShopRepo shopRepo)
{
    private readonly ShopRepo _shopRepo = shopRepo;

    public async Task<IEnumerable<ShopItem>> GetAllAsync()
    {
        return await _shopRepo.GetAllAsync();
    }

    public async Task<ShopItem> GetByIDAsync(int ID)
    {
        return await _shopRepo.GetByIDAsync(ID);
    }

    public async Task AddAsync(ShopItem shopItem)
    {
        await _shopRepo.AddAsync(shopItem);
    }

    internal async Task<string> AddItemToCartAsync(int itemID, string userToken)
    {
        return await _shopRepo.AddItemToCartAsync(itemID, userToken);
    }
}