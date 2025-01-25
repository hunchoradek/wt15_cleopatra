using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Cleopatra.Domain;
using Cleopatra.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Salon.Domain;
using Cleopatra.API.Services;

namespace Cleopatra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly SalonContext _context;

        public EmployeesController(SalonContext context)
        {
            _context = context;
        }

        // GET: api/employees
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/employees/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // POST: api/employees
        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            // Sprawdź, czy model jest poprawny
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obsługa hashowania hasła
            employee.password_hash = BCrypt.Net.BCrypt.HashPassword(employee.password_hash);

            // Serializacja `working_hours` do JSON, jeśli to string
            if (employee.working_hours is string)
            {
                try
                {
                    Newtonsoft.Json.Linq.JToken.Parse(employee.working_hours);
                }
                catch (Exception)
                {
                    return BadRequest("Invalid JSON format for working_hours.");
                }
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.employee_id }, employee);
        }


        // PUT: api/employees/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee updatedEmployee)
        {
            // Znajdź istniejącego pracownika na podstawie ID
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            // Aktualizacja danych pracownika
            employee.name = updatedEmployee.name;
            employee.email = updatedEmployee.email;
            employee.username = updatedEmployee.username;
            employee.phone_number = updatedEmployee.phone_number;
            employee.role = updatedEmployee.role;

            // Jeśli hasło zostało podane, zaktualizuj je
            if (!string.IsNullOrEmpty(updatedEmployee.password_hash))
            {
                employee.password_hash = BCrypt.Net.BCrypt.HashPassword(updatedEmployee.password_hash);
            }

            // Aktualizacja godzin pracy (working_hours)
            if (updatedEmployee.working_hours != null)
            {
                if (updatedEmployee.working_hours is string)
                {
                    // Walidacja JSON, jeśli working_hours jest stringiem
                    try
                    {
                        Newtonsoft.Json.Linq.JToken.Parse(updatedEmployee.working_hours.ToString());
                        employee.working_hours = updatedEmployee.working_hours;
                    }
                    catch (Exception)
                    {
                        return BadRequest("Invalid JSON format for working_hours.");
                    }
                }
            }

            // Aktualizacja specjalności (specialties)
            employee.specialties = updatedEmployee.specialties;

            // Flaga usunięcia
            employee.isDeleted = updatedEmployee.isDeleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound($"Employee with ID {id} no longer exists.");
                }
                throw;
            }

            return NoContent(); // Zwróć status 204 (brak treści)
        }


        // PUT: api/employees/{id}/change-password
        [HttpPut("{id}/change-password")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] string newPassword)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.password_hash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/employees/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            // Pobranie pracownika
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == id);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            // Ustawienie isDeleted na true
            employee.isDeleted = true;

            // Zmiana statusu wszystkich spotkań pracownika na "cancelled"
            var appointments = _context.Appointments.Where(a => a.employee_id == id && a.status != "cancelled").ToList();
            foreach (var appointment in appointments)
            {
                appointment.status = "cancelled";
            }

            // Zapisanie zmian
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/employees/{id}/vacations
        [HttpPost("{id}/vacations")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> AddVacation(int id, [FromBody] VacationRequest request)
        {
            // Sprawdź, czy pracownik istnieje
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new { Message = "Pracownik nie został znaleziony." });
            }

            // Utwórz nowy wpis urlopu
            var vacation = new Vacation
            {
                employee_id = id,
                start_date = request.StartDate,
                end_date = request.EndDate
            };
            _context.Vacations.Add(vacation);

            // Anulowanie rezerwacji w okresie urlopu
            var appointments = _context.Appointments
                .Where(a => a.employee_id == id &&
                            a.appointment_date >= request.StartDate &&
                            a.appointment_date <= request.EndDate);

            foreach (var appointment in appointments)
            {
                appointment.status = "cancelled";

                //var client = await _context.Clients.FindAsync(appointment.client_id);
                //if (client != null && !string.IsNullOrEmpty(client.email))
                //{
                    //await _notificationService.SendEmailAsync(
                        //client.email,
                        //"Anulowanie rezerwacji",
                        //$"Twoje spotkanie zaplanowane na {appointment.appointment_date:yyyy-MM-dd} zostało anulowane z powodu urlopu pracownika."
                    //);
                //}
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // GET: api/employees/available-times
        [HttpGet("available-times")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableTimes(DateTime date, int? employeeId = null)
        {
            var employeesQuery = employeeId.HasValue
                ? _context.Employees.Where(e => e.employee_id == employeeId)
                : _context.Employees;

            var employees = await employeesQuery.ToListAsync(); // Pobierz pracowników jednorazowo

            var availability = new List<object>();

            foreach (var employee in employees)
            {
                // Wywołanie CalculateAvailableHours dla każdego pracownika
                var availableHours = CalculateAvailableHours(employee.working_hours, date, employee.employee_id);

                availability.Add(new
                {
                    employee_id = employee.employee_id,
                    name = employee.name,
                    available_hours = availableHours
                });
            }

            return Ok(availability);
        }

        // GET: api/employees/{id}/schedule
        [HttpGet("{id}/schedule")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<IEnumerable<object>>> GetSchedule(int id, DateTime date)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == id);
            if (employee == null)
            {
                return NotFound();
            }

            // Tylko rezerwacje danego pracownika w danym dniu
            var schedule = _context.Appointments
                .Where(a => a.employee_id == id && a.appointment_date == date)
                .Select(a => new
                {
                    a.appointment_id,
                    a.service,
                    a.start_time,
                    a.end_time,
                    a.status
                });

            return Ok(await schedule.ToListAsync());
        }

        // GET: api/employees/me/schedule
        [HttpGet("me/schedule")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetMySchedule(DateTime date)
        {
            // Pobranie employee_id z ClaimsPrincipal
            var employeeIdClaim = User.Claims.FirstOrDefault(c => c.Type == "employee_id");
            if (employeeIdClaim == null)
            {
                return Unauthorized("Employee ID not found in claims.");
            }

            if (!int.TryParse(employeeIdClaim.Value, out int employeeId))
            {
                return Unauthorized("Invalid Employee ID in claims.");
            }

            // Pobranie pracownika z bazy danych
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == employeeId);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            // Pobranie harmonogramu dla zalogowanego pracownika w danym dniu
            var schedule = _context.Appointments
                .Where(a => a.employee_id == employeeId && a.appointment_date == date.Date)
                .Select(a => new
                {
                    a.appointment_id,
                    a.service,
                    a.start_time,
                    a.end_time,
                    a.status
                });

            return Ok(await schedule.ToListAsync());
        }


        // Pomocnicza funkcja do obliczania dostępnych godzin pracy
        private List<string> CalculateAvailableHours(string workingHoursJson, DateTime date, int employeeId)
        {
            // 1. Parsowanie JSON-a harmonogramu pracy
            var workingHours = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(workingHoursJson);

            // Pobranie odpowiedniego dnia tygodnia (np. "mon", "tue")
            string dayKey = date.DayOfWeek.ToString().ToLower().Substring(0, 3);
            if (!workingHours.ContainsKey(dayKey))
            {
                // Jeśli brak godzin pracy w danym dniu, pracownik nie pracuje
                return new List<string>();
            }

            // Lista przedziałów czasowych w formacie "09:00-17:00"
            var workIntervals = workingHours[dayKey];

            // 2. Pobranie rezerwacji dla pracownika w danym dniu (jednorazowo!)
            var appointments = _context.Appointments
                .Where(a => a.employee_id == employeeId && a.appointment_date == date)
                .Select(a => new { a.start_time, a.end_time }) // Tylko potrzebne kolumny
                .ToList(); // Pobieramy dane z bazy, aby nie trzymać otwartego połączenia

            // 3. Obliczanie dostępnych godzin
            var availableHours = new List<string>();

            foreach (var interval in workIntervals)
            {
                // Parsowanie godzin pracy (np. "09:00-17:00" → TimeSpan)
                var times = interval.Split('-');
                var start = TimeSpan.Parse(times[0]);
                var end = TimeSpan.Parse(times[1]);

                // Rozpocznij od początku przedziału i sprawdzaj dostępność
                var currentTime = start;
                while (currentTime < end)
                {
                    var nextTime = currentTime.Add(TimeSpan.FromMinutes(30)); // Krok co 30 minut
                    if (nextTime > end) break;

                    // Sprawdź, czy czas jest wolny
                    bool isOccupied = appointments.Any(a =>
                        a.start_time < nextTime && a.end_time > currentTime);

                    if (!isOccupied)
                    {
                        availableHours.Add($"{currentTime:hh\\:mm}-{nextTime:hh\\:mm}");
                    }

                    currentTime = nextTime; // Przejdź do kolejnego przedziału
                }
            }

            return availableHours;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.employee_id == id);
        }


    }
}
