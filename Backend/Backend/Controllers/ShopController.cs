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
    
    [HttpPost]
    public async Task<IActionResult> AddShopItemAsync(ShopItem shopItem)
    {
        await _shopService.AddAsync(shopItem);
        if (shopItem == null)
        {
            return NotFound();
        }
        return Ok(shopItem);
    }

    [HttpPost("addToCart/{itemID}/{userToken}")]
    public async Task<IActionResult> AddItemToCartAsync(int itemID, string userToken)
    {
        var result = await _shopService.AddItemToCartAsync(itemID, userToken);

        if (result == "ITEM_ALREADY_IN_CART")
        {
            return Conflict(new { code = "ITEM_ALREADY_IN_CART", message = "This item is already in your cart." });
        }

        if (result == "ITEM_NOT_FOUND")
        {
            return NotFound(new { code = "ITEM_NOT_FOUND", message = "Item does not exist." });
        }

        return Ok(new { code = "ITEM_ADDED", message = "Item added to cart." });
    }
}