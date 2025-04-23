using System.ComponentModel.DataAnnotations;

namespace Models;

public class User
{   [Key]
    public string token { get; set; } = "";
    public string? UserName { get; set; }
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
}