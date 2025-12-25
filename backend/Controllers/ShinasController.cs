using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShinasController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ShinasController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Shinas.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.Shinas.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Models.Shina shina)
        {
            if (shina == null) return BadRequest();
            if (string.IsNullOrWhiteSpace(shina.Name) || string.IsNullOrWhiteSpace(shina.Brand)) return BadRequest("Name and Brand required");
            _db.Shinas.Add(shina);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = shina.Id }, shina);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Shinas.FindAsync(id);
            if (item == null) return NotFound();
            _db.Shinas.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
