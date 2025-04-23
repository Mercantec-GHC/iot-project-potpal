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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPost]public async Task<IActionResult> AddUserAsync(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
}