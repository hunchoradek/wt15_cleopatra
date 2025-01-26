using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Infrastructure;
using Cleopatra.Domain;

[Route("api/[controller]")]
[ApiController]
public class VacationsController : ControllerBase
{
    private readonly SalonContext _context;
    private readonly EmailService _emailService;

    public VacationsController(SalonContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // GET: api/vacations
    [HttpGet]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<ActionResult<IEnumerable<Vacation>>> GetVacations()
    {
        return Ok(await _context.Vacations.Include(v => v.Employee).ToListAsync());
    }

    // GET: api/vacations/employee/{employeeId}
    [HttpGet("employee/{employeeId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Vacation>>> GetVacationsByEmployee(int employeeId)
    {
        var vacations = await _context.Vacations
            .Where(v => v.employee_id == employeeId)
            .ToListAsync();

        if (!vacations.Any())
        {
            return NotFound($"No vacations found for employee with ID {employeeId}.");
        }

        return Ok(vacations);
    }

    // GET: api/vacations/{id}
    [HttpGet("{id}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<ActionResult<Vacation>> GetVacation(int id)
    {
        var vacation = await _context.Vacations.Include(v => v.Employee).FirstOrDefaultAsync(v => v.vacation_id == id);

        if (vacation == null)
        {
            return NotFound($"Vacation with ID {id} not found.");
        }

        return Ok(vacation);
    }

    // POST: api/vacations
    [HttpPost]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<ActionResult<Vacation>> AddVacation(Vacation vacation)
    {
        // Sprawdzenie, czy istnieje pracownik
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == vacation.employee_id);
        if (employee == null)
        {
            return NotFound($"Employee with ID {vacation.employee_id} not found.");
        }

        // Dodanie urlopu
        _context.Vacations.Add(vacation);

        // Anulowanie rezerwacji w czasie urlopu
        var appointments = _context.Appointments
            .Where(a => a.employee_id == vacation.employee_id &&
                        a.appointment_date >= vacation.start_date &&
                        a.appointment_date <= vacation.end_date);

        foreach (var appointment in appointments)
        {
            appointment.status = "cancelled";

            // Extract client email from the database
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.client_id == appointment.client_id);
            if (client != null)
            {
                var clientEmail = client.email; // Assuming the Client entity has an email property
                var subject = "Salon Cleopatra - Spotkanie anulowane";
                var body = $"Szanowny/a {client.name},\n\nTwoje spotkanie zaplanowane na {appointment.appointment_date} zostało anulowane ze względu na urlop pracownika.\n\nPrzepraszamy za niedogodności.\n\nZ wyrazami szacunku,\nSalon Cleopatra";

                await _emailService.SendEmailAsync(clientEmail, subject, body);
            }
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVacation), new { id = vacation.vacation_id }, vacation);
    }

    // PUT: api/vacations/{id}
    [HttpPut("{id}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> UpdateVacation(int id, Vacation updatedVacation)
    {
        if (id != updatedVacation.vacation_id)
        {
            return BadRequest("Vacation ID mismatch.");
        }

        var vacation = await _context.Vacations.FirstOrDefaultAsync(v => v.vacation_id == id);
        if (vacation == null)
        {
            return NotFound($"Vacation with ID {id} not found.");
        }

        vacation.start_date = updatedVacation.start_date;
        vacation.end_date = updatedVacation.end_date;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/vacations/{id}
    [HttpDelete("{id}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> DeleteVacation(int id)
    {
        var vacation = await _context.Vacations.FirstOrDefaultAsync(v => v.vacation_id == id);
        if (vacation == null)
        {
            return NotFound($"Vacation with ID {id} not found.");
        }

        _context.Vacations.Remove(vacation);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
