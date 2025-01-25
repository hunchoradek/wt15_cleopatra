using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Infrastructure;
using System;
using System.Threading.Tasks;
using Cleopatra.API.Services;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly SalonContext _context;
    private readonly NotificationService _notificationService;

    public NotificationsController(SalonContext context, NotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Wysyłanie przypomnienia o konkretnym spotkaniu.
    /// </summary>
    /// <param name="appointmentId">ID spotkania.</param>
    /// <returns>HTTP 200, jeśli wysłano przypomnienie.</returns>
    [HttpPost("send-reminder/{appointmentId}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> SendReminder(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.appointment_id == appointmentId);

        if (appointment == null)
        {
            return NotFound("Appointment not found.");
        }

        if (appointment.Client == null)
        {
            return BadRequest("Client information is missing for this appointment.");
        }

        var client = appointment.Client;
        var subject = "Appointment Reminder";
        var body = $@"
            <p>Dear {client.name},</p>
            <p>This is a reminder for your upcoming appointment.</p>
            <p><b>Details:</b></p>
            <ul>
                <li><b>Service:</b> {appointment.service}</li>
                <li><b>Date:</b> {appointment.appointment_date:yyyy-MM-dd}</li>
                <li><b>Time:</b> {appointment.start_time} - {appointment.end_time}</li>
            </ul>
            <p>We look forward to seeing you!</p>";

        await _notificationService.SendEmailAsync(client.email, subject, body);

        return Ok("Reminder sent successfully.");
    }

    /// <summary>
    /// Wysyłanie automatycznych przypomnień dla wszystkich klientów.
    /// </summary>
    /// <returns>HTTP 200, jeśli wysłano przypomnienia.</returns>
    [HttpPost("send-daily-reminders")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> SendDailyReminders()
    {
        var tomorrow = DateTime.Now.Date.AddDays(1);

        var appointments = await _context.Appointments
            .Include(a => a.Client)
            .Where(a => a.appointment_date == tomorrow && a.status == "scheduled")
            .ToListAsync();

        foreach (var appointment in appointments)
        {
            if (appointment.Client == null)
                continue;

            var client = appointment.Client;
            var subject = "Appointment Reminder";
            var body = $@"
                <p>Dear {client.name},</p>
                <p>This is a reminder for your appointment scheduled tomorrow.</p>
                <p><b>Details:</b></p>
                <ul>
                    <li><b>Service:</b> {appointment.service}</li>
                    <li><b>Date:</b> {appointment.appointment_date:yyyy-MM-dd}</li>
                    <li><b>Time:</b> {appointment.start_time} - {appointment.end_time}</li>
                </ul>
                <p>We look forward to seeing you!</p>";

            await _notificationService.SendEmailAsync(client.email, subject, body);
        }

        return Ok("Daily reminders sent successfully.");
    }

    /// <summary>
    /// Powiadomienie o anulowaniu spotkania.
    /// </summary>
    /// <param name="appointmentId">ID spotkania.</param>
    /// <returns>HTTP 200, jeśli powiadomienie wysłano.</returns>
    [HttpPost("send-cancellation/{appointmentId}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> SendCancellationNotification(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.appointment_id == appointmentId);

        if (appointment == null)
        {
            return NotFound("Appointment not found.");
        }

        if (appointment.Client == null)
        {
            return BadRequest("Client information is missing for this appointment.");
        }

        var client = appointment.Client;
        var subject = "Appointment Cancellation";
        var body = $@"
            <p>Dear {client.name},</p>
            <p>We regret to inform you that your appointment has been cancelled.</p>
            <p><b>Details:</b></p>
            <ul>
                <li><b>Service:</b> {appointment.service}</li>
                <li><b>Date:</b> {appointment.appointment_date:yyyy-MM-dd}</li>
                <li><b>Time:</b> {appointment.start_time} - {appointment.end_time}</li>
            </ul>
            <p>Please contact us to reschedule if needed.</p>";

        await _notificationService.SendEmailAsync(client.email, subject, body);

        return Ok("Cancellation notification sent successfully.");
    }
}
