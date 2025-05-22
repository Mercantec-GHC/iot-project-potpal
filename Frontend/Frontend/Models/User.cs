using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace Models;

public class User
{   
    public string Email { get; set; }
    public string? UserName { get; set; }
    public string Token { get; set; } 
}
