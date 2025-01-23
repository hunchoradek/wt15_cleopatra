using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Domain;
using Cleopatra.Infrastructure;

namespace Cleopatra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCategoriesController : ControllerBase
    {
        private readonly SalonContext _context;

        public ServiceCategoriesController(SalonContext context)
        {
            _context = context;
        }

        // GET: api/servicecategories
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<IEnumerable<ServiceCategory>>> GetCategories()
        {
            return await _context.ServiceCategories.ToListAsync();
        }

        // GET: api/servicecategories/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<ServiceCategory>> GetCategory(int id)
        {
            var category = await _context.ServiceCategories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/servicecategories
        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<ServiceCategory>> AddCategory(ServiceCategory category)
        {
            _context.ServiceCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.category_id }, category);
        }

        // DELETE: api/servicecategories/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.ServiceCategories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.ServiceCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
