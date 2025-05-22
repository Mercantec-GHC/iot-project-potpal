using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserLoginDTO
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
