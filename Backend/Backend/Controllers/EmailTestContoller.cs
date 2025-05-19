using Database;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EmailTestController : Controller
{

    private readonly EmailService _emailService;

    public EmailTestController(EmailService emailService)
    {
        _emailService = emailService;

    }

    [HttpGet]
    public async Task<IActionResult> GetAllMetrics()
    {
        var email = new Email
        {
            Recipient = "eva.lypak@gmail.com",
            Subject = "Test email",
            Body = "<H1>This is a test email</H1><p>Test</p>"
        };

        await _emailService.SendAsync(email);
        
        return NoContent();
    }

}