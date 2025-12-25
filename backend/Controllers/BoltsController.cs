using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoltsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BoltsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Bolts.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.Bolts.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Models.Bolt bolt)
        {
            if (bolt == null) return BadRequest();
            if (string.IsNullOrWhiteSpace(bolt.Name) || string.IsNullOrWhiteSpace(bolt.Brand)) return BadRequest("Name and Brand required");
            _db.Bolts.Add(bolt);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = bolt.Id }, bolt);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Bolts.FindAsync(id);
            if (item == null) return NotFound();
            _db.Bolts.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
