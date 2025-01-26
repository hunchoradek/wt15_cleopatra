using FluentEmail.Core;
using FluentEmail.Core.Models;

public class EmailService
{
    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task<SendResponse> SendEmailAsync(string toEmail, string subject, string body)
    {
        var response = await _fluentEmail
            .To(toEmail)
            .Subject(subject)
            .Body(body, isHtml: true) // Ustaw `isHtml: true`, jeśli body ma HTML
            .SendAsync();

        return response;
    }
}