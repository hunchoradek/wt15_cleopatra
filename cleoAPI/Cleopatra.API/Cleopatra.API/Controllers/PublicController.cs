using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Domain;
using Cleopatra.Infrastructure;
using Cleopatra.API.Helpers;
using Newtonsoft.Json;

namespace Cleopatra.API.Controllers
{
    [Route("api/public")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly SalonContext _context;

        public PublicController(SalonContext context)
        {
            _context = context;
        }

        // GET: /api/public/service-categories
        [HttpGet("service-categories")]
        public async Task<IActionResult> GetServiceCategories()
        {
            var categories = await _context.ServiceCategories
                .Select(c => new { c.category_id, c.name })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: /api/public/services/{categoryId}
        [HttpGet("services/{categoryId}")]
        public async Task<IActionResult> GetServicesByCategory(int categoryId)
        {
            var services = await _context.Services
                .Where(s => s.category_id == categoryId)
                .Select(s => new { s.service_id, s.name, s.description, s.duration, s.price })
                .ToListAsync();

            if (!services.Any())
            {
                return NotFound("No services found for the given category.");
            }

            return Ok(services);
        }

        // GET: /api/public/employees/{categoryId}
        [HttpGet("employees/{categoryId}")]
        public async Task<IActionResult> GetEmployeesByCategory(int categoryId)
        {
            var categoryString = $"\"{categoryId}\"";

            var employees = await _context.Employees
                .Where(e => e.specialties.Contains(categoryString) && e.isDeleted == false)
                .Select(e => new { e.employee_id, e.name, e.phone_number })
                .ToListAsync();

            if (!employees.Any())
            {
                return NotFound("No employees found for the given category.");
            }

            return Ok(employees);
        }



        // GET: /api/public/available-times
        [HttpGet("available-times")]
        public async Task<IActionResult> GetAvailableTimes([FromQuery] DateTime date, [FromQuery] int? employeeId = null, [FromQuery] int? categoryId = null)
        {
            if (employeeId.HasValue)
            {
                var employee = await _context.Employees.FindAsync(employeeId.Value);
                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                var availableHours = CalculateAvailableHours(employee.working_hours, date, employee.employee_id);

                return Ok(new
                {
                    employee_id = employee.employee_id,
                    name = employee.name,
                    available_hours = availableHours
                });
            }
            else if (categoryId.HasValue)
            {
                var employees = await _context.Employees
                    .Where(e => !e.isDeleted && e.role == "worker")
                    .ToListAsync();

                var availability = new List<object>();

                foreach (var employee in employees)
                {
                    var specialties = JsonConvert.DeserializeObject<List<int>>(employee.specialties);
                    if (specialties.Contains(categoryId.Value))
                    {
                        var availableHours = CalculateAvailableHours(employee.working_hours, date, employee.employee_id);

                        availability.Add(new
                        {
                            employee_id = employee.employee_id,
                            name = employee.name,
                            available_hours = availableHours
                        });
                    }
                }

                return Ok(availability);
            }
            else
            {
                return BadRequest("Either employeeId or categoryId must be provided.");
            }
        }


        // POST: /api/public/appointments
        [HttpPost("appointments")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ClientPhoneNumber))
            {
                return BadRequest("Client phone number is required.");
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.phone_number == request.ClientPhoneNumber);

            int clientId;

            if (client != null)
            {
                clientId = client.client_id;
            }
            else
            {
                var newClient = new Client
                {
                    name = request.ClientName,
                    phone_number = request.ClientPhoneNumber,
                    email = request.ClientEmail,
                    notes = request.ClientNotes,
                    is_deleted = false
                };

                _context.Clients.Add(newClient);
                await _context.SaveChangesAsync();

                clientId = newClient.client_id;
            }

            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            var service = await _context.Services.FindAsync(request.ServiceId);

            if (employee == null || service == null)
            {
                return BadRequest("Invalid employee or service.");
            }

            var existingAppointment = await _context.Appointments
                .AnyAsync(a => a.employee_id == request.EmployeeId &&
                               a.appointment_date == request.AppointmentDate &&
                               a.start_time == request.StartTime);

            if (existingAppointment)
            {
                return Conflict("The selected time slot is already booked.");
            }

            var appointment = new Appointment
            {
                client_id = clientId,
                employee_id = request.EmployeeId,
                employee_name = employee.name,
                service = service.name,
                appointment_date = request.AppointmentDate,
                start_time = request.StartTime,
                end_time = request.StartTime + TimeSpan.FromMinutes(service.duration),
                status = "scheduled",
                notes = request.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Appointment successfully created.", AppointmentId = appointment.appointment_id });
        }

        private List<string> CalculateAvailableHours(string workingHoursJson, DateTime date, int employeeId)
        {
            // 1. Sprawdź, czy pracownik ma urlop w danym dniu
            var hasVacation = _context.Vacations
                .Any(v => v.employee_id == employeeId && date >= v.start_date && date <= v.end_date);

            if (hasVacation)
            {
                // Jeśli pracownik ma urlop, nie ma dostępnych godzin
                return new List<string>();
            }

            // 2. Parse the working hours JSON
            var workingHours = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(workingHoursJson);

            // Get the appropriate day of the week (e.g., "mon", "tue")
            string dayKey = date.DayOfWeek.ToString().ToLower().Substring(0, 3);
            if (!workingHours.ContainsKey(dayKey))
            {
                // If no working hours for the day, the employee is not working
                return new List<string>();
            }

            // List of time intervals in the format "09:00-17:00"
            var workIntervals = workingHours[dayKey];

            // 3. Fetch appointments for the employee on the given day (once!)
            var appointments = _context.Appointments
                .Where(a => a.employee_id == employeeId && a.appointment_date == date)
                .Select(a => new { a.start_time, a.end_time }) // Only needed columns
                .ToList(); // Fetch data from the database to avoid keeping the connection open

            // 4. Calculate available hours
            var availableHours = new List<string>();

            foreach (var interval in workIntervals)
            {
                // Parse working hours (e.g., "09:00-17:00" → TimeSpan)
                var times = interval.Split('-');
                var start = TimeSpan.Parse(times[0]);
                var end = TimeSpan.Parse(times[1]);

                // Start from the beginning of the interval and check availability
                var currentTime = start;
                while (currentTime < end)
                {
                    var nextTime = currentTime.Add(TimeSpan.FromMinutes(30)); // 30-minute step
                    if (nextTime > end) break;

                    // Check if the time is free
                    bool isOccupied = appointments.Any(a =>
                        a.start_time < nextTime && a.end_time > currentTime);

                    if (!isOccupied)
                    {
                        availableHours.Add($"{currentTime:hh\\:mm}-{nextTime:hh\\:mm}");
                    }

                    currentTime = nextTime; // Move to the next interval
                }
            }

            return availableHours;
        }
    }



    // Klasa pomocnicza dla żądania rezerwacji
    public class AppointmentRequest
    {
        public string ClientName { get; set; } // Imię klienta
        public string ClientPhoneNumber { get; set; } // Telefon klienta
        public string ClientEmail { get; set; } // Opcjonalny email klienta
        public string ClientNotes { get; set; } // Opcjonalne notatki dotyczące klienta
        public int ServiceId { get; set; } // Usługa
        public int EmployeeId { get; set; } // Pracownik
        public DateTime AppointmentDate { get; set; } // Data wizyty
        public TimeSpan StartTime { get; set; } // Godzina rozpoczęcia wizyty
        public string Notes { get; set; } // Opcjonalne notatki dotyczące wizyty
    }

}
