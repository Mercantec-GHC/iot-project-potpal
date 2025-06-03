namespace Models;

public class CartItem
{
    public string UserEmail { get; set; } = null!;
    public int ItemId { get; set; }

    public User User { get; set; } = null!;
    public ShopItem ShopItem { get; set; } = null!;

    public int Quantity { get; set; } = 1;
}