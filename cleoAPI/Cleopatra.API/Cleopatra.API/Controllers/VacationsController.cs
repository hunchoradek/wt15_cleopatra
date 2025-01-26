﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Infrastructure;
using Cleopatra.Domain;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "ManagerOnly")]
public class VacationsController : ControllerBase
{
    private readonly SalonContext _context;

    public VacationsController(SalonContext context)
    {
        _context = context;
    }

    // GET: api/vacations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vacation>>> GetVacations()
    {
        return Ok(await _context.Vacations.Include(v => v.Employee).ToListAsync());
    }

    // GET: api/vacations/employee/{employeeId}
    [HttpGet("employee/{employeeId}")]
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
            // Możesz dodać wysyłanie powiadomień e-mail do klientów
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVacation), new { id = vacation.vacation_id }, vacation);
    }

    // PUT: api/vacations/{id}
    [HttpPut("{id}")]
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
