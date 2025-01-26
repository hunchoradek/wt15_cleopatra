using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Infrastructure;
using Cleopatra.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Cleopatra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly SalonContext _context;
        private readonly EmailService _emailService;

        public AppointmentsController(SalonContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/appointments
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Client)     // Pobiera powiązane dane klienta
                .Include(a => a.Employee)  // Pobiera powiązane dane pracownika
                .ToListAsync();

            return Ok(appointments);
        }

        // GET: api/appointments/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.appointment_id == id);

            if (appointment == null) return NotFound();

            return appointment;
        }

        // POST: api/appointments
        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Appointment>> AddAppointment(Appointment appointment)
        {
            // Walidacja dostępności pracownika w podanym przedziale czasowym
            var conflictingAppointment = _context.Appointments
                .FirstOrDefault(a =>
                    a.employee_id == appointment.employee_id &&
                    a.appointment_date == appointment.appointment_date &&
                    (
                        (appointment.start_time >= a.start_time && appointment.start_time < a.end_time) || // Kolizja na początku
                        (appointment.end_time > a.start_time && appointment.end_time <= a.end_time) ||   // Kolizja na końcu
                        (appointment.start_time <= a.start_time && appointment.end_time >= a.end_time)   // Pełne pokrycie
                    )
                );

            if (conflictingAppointment != null)
            {
                return BadRequest(new
                {
                    Message = "Pracownik nie jest dostępny w tym terminie.",
                    ConflictingAppointment = new
                    {
                        conflictingAppointment.appointment_id,
                        conflictingAppointment.start_time,
                        conflictingAppointment.end_time,
                        conflictingAppointment.appointment_date
                    }
                });
            }

            // Dodajemy nowe spotkanie
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.appointment_id }, appointment);
        }


        // PUT: api/appointments/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
        {
            if (id != appointment.appointment_id) return BadRequest();

            // Walidacja dostępności podczas aktualizacji
            var isAvailable = !_context.Appointments.Any(a =>
                a.appointment_id != id &&
                a.employee_id == appointment.employee_id &&
                a.appointment_date == appointment.appointment_date &&
                a.start_time < appointment.end_time &&
                a.end_time > appointment.start_time);

            if (!isAvailable)
            {
                return BadRequest("Pracownik nie jest dostępny w tym terminie.");
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/appointments/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Client) // Include the Client entity
                .FirstOrDefaultAsync(a => a.appointment_id == id);

            if (appointment == null) return NotFound();

            // Extract client email from the database
            var client = appointment.Client;
            if (client != null)
            {
                var clientEmail = client.email; // Assuming the Client entity has an email property
                var subject = "Salon Cleopatra - Spotkanie anulowane";
                var body = $"Szanowny/a {client.name},\n\nTwoje spotkanie zaplanowane na {appointment.appointment_date} zostało anulowane.\n\nPrzepraszamy za niedogodności.\n\nZ wyrazami szacunku,\nSalon Cleopatra";

                await _emailService.SendEmailAsync(clientEmail, subject, body);
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Helper: Sprawdzanie, czy rezerwacja istnieje
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.appointment_id == id);
        }
    }
}
