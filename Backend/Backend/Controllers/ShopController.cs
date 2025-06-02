using Database;
using Microsoft.AspNetCore.Mvc;
using Models;
using Stripe;
using Stripe.Checkout;

namespace Backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ShopController : Controller
{

    private readonly ShopService _shopService;
    private readonly ILogger<ShopController> _logger;

    public ShopController(ShopService shopService, ILogger<ShopController> logger)
    {
        _shopService = shopService;
        _logger = logger;
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

    public class CheckoutRequest
    {
        public string UserToken { get; set; } = string.Empty;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CheckoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserToken))
            return BadRequest(new { error = "User token is required." });

        var cartItems = await _shopService.GetCartByUserAsync(request.UserToken);

        if (cartItems == null || !cartItems.Any())
            return BadRequest(new { error = "Cart is empty." });

        // Calculate total amount (assumes price is in DKK)
        var totalAmount = cartItems.Sum(item => item.ShopItem.Price * item.Quantity);
        var amountInÿre = (long)(totalAmount * 100); // Stripe expects smallest currency unit

        var options = new PaymentIntentCreateOptions
        {
            Amount = amountInÿre,
            Currency = "dkk",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        try
        {
            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return Ok(new { ClientSecret = intent.ClientSecret });
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "StripeException while creating PaymentIntent: {Message}", ex.Message);
            return StatusCode(500, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in CreatePaymentIntent");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

}