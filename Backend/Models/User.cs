using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace Models;

public class User
{
    [Key]
    public string token { get; set; } = "";
    public string? UserName { get; set; }
    public string Password { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}
