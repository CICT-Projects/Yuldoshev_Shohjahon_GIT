using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MotorsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Motors.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.Motors.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Models.Motor motor)
        {
            if (motor == null) return BadRequest();
            if (string.IsNullOrWhiteSpace(motor.Name) || string.IsNullOrWhiteSpace(motor.Brand)) return BadRequest("Name and Brand required");
            _db.Motors.Add(motor);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = motor.Id }, motor);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Motors.FindAsync(id);
            if (item == null) return NotFound();
            _db.Motors.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
