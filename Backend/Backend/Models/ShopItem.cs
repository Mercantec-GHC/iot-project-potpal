
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models;

public class ShopItem
{
    [Key]
    public int Id { get; set; }
    public string ItemName { get; set; } = "";
    public string ImagePath { get; set; } = "";
    public string Description { get; set; } = "";
    public float Price { get; set; }

    public List<User> UsersWithThisInCart { get; set; } = new ();
}