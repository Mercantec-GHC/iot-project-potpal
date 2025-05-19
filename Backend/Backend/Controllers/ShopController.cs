using Database;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ShopController : Controller
{
   
    private readonly ShopService _shopService;

    public ShopController(ShopService shopService)
    {
        _shopService = shopService;

    }

    [HttpGet]
    public async Task<IActionResult> GetAllShopItems()
    {
        var metric = await _shopService.GetAllAsync();
        if (metric == null)
        {
            return NotFound();
        }
        return Ok(metric);
    }

    [HttpGet("byID/{ID}")]
    public async Task<IActionResult> GetShopItemByID(int ID)
    {
        var metric = await _shopService.GetByIDAsync(ID);
        if (metric == null)
        {
            return NotFound();
        }
        return Ok(metric);
    }
    
    [HttpPost]public async Task<IActionResult> AddShopItemAsync(ShopItem shopItem)
    {
        await _shopService.AddAsync(shopItem);
        if (shopItem == null)
        {
            return NotFound();
        }
        return Ok(shopItem);
    }
}