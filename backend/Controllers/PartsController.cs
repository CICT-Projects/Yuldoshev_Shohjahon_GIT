using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PartsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parts = await _db.Parts.AsNoTracking().ToListAsync();
            return Ok(parts);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var part = await _db.Parts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (part == null) return NotFound();
            return Ok(part);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Part part)
        {
            if (part == null) return BadRequest();
            if (string.IsNullOrWhiteSpace(part.Name) || string.IsNullOrWhiteSpace(part.Category)) return BadRequest("Name and Category are required.");
            _db.Parts.Add(part);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = part.Id }, part);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Part updated)
        {
            if (updated == null) return BadRequest();
            if (id != updated.Id) return BadRequest();
            if (string.IsNullOrWhiteSpace(updated.Name) || string.IsNullOrWhiteSpace(updated.Category)) return BadRequest("Name and Category are required.");
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();

            part.Name = updated.Name;
            part.Category = updated.Category;
            part.Price = updated.Price;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();

            _db.Parts.Remove(part);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
