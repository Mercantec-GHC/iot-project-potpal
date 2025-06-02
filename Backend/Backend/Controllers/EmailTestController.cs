using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmailTestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailTestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendTestEmail([FromBody] EmailRequest request)
    {
        await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
        return Ok("Email sent.");
    }
}

public class EmailRequest
{
    public string To { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Body { get; set; } = "";
}
