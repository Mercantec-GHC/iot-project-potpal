using Database;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
   
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;

    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var user = await _userService.GetAllAsync();
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> GetUserByToken(string token)
    {
        var user = await _userService.GetByTokenAsync(token);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddUserAsync(CreateUserDTO createUser)
    {
        UserDTO NewUser = await _userService.AddAsync(createUser);
        if (NewUser == null)
        {
            return NotFound();
        }
        return Ok(NewUser);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(UserLoginDTO LoginUser)
    {
        UserDTO user = await _userService.LoginAsync(LoginUser);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

}