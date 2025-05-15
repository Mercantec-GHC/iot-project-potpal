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


    public async Task AddAsync(ShopItem shopItem)
    {
        await _shopRepo.AddAsync(shopItem);
    }
}