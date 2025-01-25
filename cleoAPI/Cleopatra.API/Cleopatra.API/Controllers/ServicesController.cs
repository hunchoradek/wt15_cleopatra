using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Domain;
using Cleopatra.Infrastructure;

namespace Cleopatra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly SalonContext _context;

        public ServicesController(SalonContext context)
        {
            _context = context;
        }

        // GET: api/services
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.Include(s => s.category).ToListAsync();
        }

        // GET: api/services/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.Include(s => s.category).FirstOrDefaultAsync(s => s.service_id == id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // POST: api/services
        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Service>> AddService(Service service)
        {
            // Usuń kategorię, jeśli jest dołączona do obiektu
            service.category = null;

            // Dodaj usługę do kontekstu
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetService), new { id = service.service_id }, service);
        }

        // PUT: api/services/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> UpdateService(int id, Service updatedService)
        {
            if (id != updatedService.service_id)
            {
                return BadRequest();
            }

            // Usuń kategorię, jeśli jest dołączona do obiektu
            updatedService.category = null;

            // Oznacz encję jako zmodyfikowaną
            _context.Entry(updatedService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Services.Any(s => s.service_id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }


        // DELETE: api/services/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
