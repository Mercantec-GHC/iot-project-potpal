namespace Models;

public class CartItem
{
    public string UserToken { get; set; } = "";
    public int ItemId { get; set; }

    public User User { get; set; } = null!;
    public ShopItem ShopItem { get; set; } = null!;

    public int Quantity { get; set; } = 1;
}