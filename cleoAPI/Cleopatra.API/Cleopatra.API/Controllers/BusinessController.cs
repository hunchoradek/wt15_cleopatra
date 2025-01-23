using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Domain;
using Cleopatra.Infrastructure;
using Salon.Domain;

namespace Cleopatra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly SalonContext _context;

        public BusinessController(SalonContext context)
        {
            _context = context;
        }

        // GET: api/business
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Business>> GetBusiness()
        {
            var business = await _context.Business.FirstOrDefaultAsync();

            if (business == null)
            {
                return NotFound();
            }

            return business;
        }

        // PUT: api/business
        [HttpPut]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> UpdateBusiness(Business updatedBusiness)
        {
            var existingBusiness = await _context.Business.FirstOrDefaultAsync();

            if (existingBusiness == null)
            {
                return NotFound();
            }

            // Aktualizujemy dane firmy
            existingBusiness.name = updatedBusiness.name;
            existingBusiness.opening_hours = updatedBusiness.opening_hours;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Błąd podczas aktualizacji danych firmy.");
            }

            return NoContent();
        }

        // POST: api/business
        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Business>> AddBusiness(Business business)
        {
            if (await _context.Business.AnyAsync())
            {
                return BadRequest("Dane firmy już istnieją. Możesz je tylko zaktualizować.");
            }

            _context.Business.Add(business);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBusiness), new { id = business.business_id }, business);
        }
    }
}
