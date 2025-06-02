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
        var product = await _shopService.GetAllAsync();
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet("byID/{ID}")]
    public async Task<IActionResult> GetShopItemByID(int ID)
    {
        var product = await _shopService.GetByIDAsync(ID);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
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

    [HttpPost("addToCart")]
    public async Task<IActionResult> AddItemToCartAsync(CartItem cart)
    {
        var result = await _shopService.AddItemToCartAsync(cart);

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

    [HttpGet("cart/byUser/{userToken}")]
    public async Task<IActionResult> GetcartByUser(string userToken)
    {
        var cartItems = await _shopService.GetCartByUserAsync(userToken);
        if (cartItems == null)
        {
            return NotFound();
        }
        return Ok(cartItems);
    }
}