using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore; // Upewnij się, że ta przestrzeń nazw jest obecna
using Cleopatra.Infrastructure;

namespace Cleopatra.API.Services
{
    public class NotificationService
    {
        private readonly SalonContext _context;
        private readonly IConfiguration _configuration;

        public NotificationService(SalonContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SendDailyReminders()
        {
            var tomorrow = DateTime.Now.Date.AddDays(1);

            var appointments = await _context.Appointments
                .Include(a => a.Client) // Metoda Include wymaga Microsoft.EntityFrameworkCore
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

                await SendEmailAsync(client.email, subject, body);
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"]; // np. "smtp.office365.com"
            var port = int.Parse(_configuration["EmailSettings:Port"]); // np. 587
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["ykbxtfmylvmhzcss"]; // App Password

            var client = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Email sending failed: {ex.Message}");
            }
        }

    }
}
