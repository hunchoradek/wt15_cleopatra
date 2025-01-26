using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cleopatra.Infrastructure;
using Cleopatra.Domain;
using Microsoft.EntityFrameworkCore;

public class AppointmentReminderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public AppointmentReminderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await SendRemindersAsync();
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Run every hour
        }
    }

    private async Task SendRemindersAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<SalonContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

            var now = DateTime.UtcNow.TimeOfDay;
            var reminderTime = now.Add(TimeSpan.FromHours(2));

            var appointments = await context.Appointments
                .Include(a => a.Client)
                .Where(a => a.start_time >= now && a.start_time <= reminderTime && !a.ReminderSent)
                .ToListAsync();

            foreach (var appointment in appointments)
            {
                var client = appointment.Client;
                if (client != null)
                {
                    var clientEmail = client.email; // Assuming the Client entity has an email property
                    var subject = "Appointment Reminder";
                    var body = $"Dear {client.name},\n\nThis is a reminder for your appointment scheduled on {appointment.start_time}.\n\nBest regards,\nSalon Cleopatra";

                    await emailService.SendEmailAsync(clientEmail, subject, body);

                    // Mark the reminder as sent
                    appointment.ReminderSent = true;
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
