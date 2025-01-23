using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Infrastructure;
using Cleopatra.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Cleopatra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly SalonContext _context;

        public ResourcesController(SalonContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            var resources = await _context.Resources.ToListAsync();
            return Ok(resources);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Resource>> GetResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null) return NotFound();
            return resource;
        }

        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Resource>> AddResource(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetResource), new { id = resource.resource_id }, resource);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> UpdateResource(int id, Resource resource)
        {
            if (id != resource.resource_id) return BadRequest();
            _context.Entry(resource).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null) return NotFound();
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
