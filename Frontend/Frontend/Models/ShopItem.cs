
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models;

public class ShopItem
{
    public int Id { get; set; }
    public string ItemName { get; set; } = "";
    public string ImagePath { get; set; } = "";
    public string Description { get; set; } = "";
    public float Price { get; set; }

    public ICollection<User> UsersWithThisInCart { get; set; } = new List<User>();
}