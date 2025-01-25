using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Infrastructure;
using Cleopatra.Domain;
using Microsoft.AspNetCore.Authorization;


namespace Cleopatra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly SalonContext _context;

        public ClientsController(SalonContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients
                .Select(c => new Client
                {
                    client_id = c.client_id,
                    name = c.name,
                    phone_number = c.phone_number ?? string.Empty,  // Handle NULL
                    email = c.email ?? string.Empty,              // Handle NULL
                    notes = c.notes ?? string.Empty               // Handle NULL
                })
                .ToListAsync();

            return Ok(clients);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return client;
        }

        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Client>> AddClient(Client client)
        {
            // Sprawdź, czy klient o podanym numerze telefonu lub e-mailu już istnieje
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.phone_number == client.phone_number || c.email == client.email);

            if (existingClient != null)
            {
                return Conflict("Client with the same phone number or email already exists.");
            }

            // Ignoruj powiązane notyfikacje
            client.Notifications = null;

            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetClient), new { id = client.client_id }, client);
            }
            catch (Exception ex)
            {
                // Obsługa nieprzewidzianych błędów
                return StatusCode(500, $"An error occurred while adding the client: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            if (id != client.client_id)
            {
                return BadRequest("Client ID in the URL does not match the body.");
            }

            var existingClient = await _context.Clients.FindAsync(id);
            if (existingClient == null)
            {
                return NotFound("Client not found.");
            }

            // Aktualizuj istniejącego klienta
            existingClient.name = client.name;
            existingClient.phone_number = client.phone_number;
            existingClient.email = client.email;
            existingClient.notes = client.notes;
            existingClient.is_deleted = client.is_deleted;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Obsługa nieprzewidzianych błędów
                return StatusCode(500, $"An error occurred while updating the client: {ex.Message}");
            }
        }



        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
