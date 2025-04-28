using System.ComponentModel.DataAnnotations;

namespace Models;

public class User
{   [Key]
    public string Email { get; set; } = "";
    public string? UserName { get; set; }
    public string Password { get; set; } = "";
}