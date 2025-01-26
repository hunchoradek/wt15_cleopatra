using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly EmailService _emailService;

    public NotificationsController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        var result = await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);

        if (result.Successful)
        {
            return Ok("Email sent successfully.");
        }

        return StatusCode(500, $"Failed to send email: {string.Join(", ", result.ErrorMessages)}");
    }
}

public class EmailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
