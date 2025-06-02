using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace Models;

public class User
{ 
    [Key]
    public string Email { get; set; } = "";

    public string Token { get; set; } = "";
    public string? UserName { get; set; }
    public string Password { get; set; } = "";
    public List<ShopItem> ItemsInCart { get; set; } = new();
    public List<Plant> plants { get; set; } = new();
}
