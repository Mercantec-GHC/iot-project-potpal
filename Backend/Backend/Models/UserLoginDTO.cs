using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }    
    }
}
