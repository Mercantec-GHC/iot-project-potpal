using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace Models;

public class User
{
 
    public string? UserName { get; set; }
    public string Password { get; set; } = "";


    [Key]
    [EmailAddress]
    public string Email { get; set; } = "";

    public List<Plant>? Plants { get; set; } = new();
    public List<CartItem> CartItems { get; set; } = new();
}