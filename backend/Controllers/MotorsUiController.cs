using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("motors-ui")]
    public class MotorsUiController : Controller
    {
        private readonly AppDbContext _db;
        public MotorsUiController(AppDbContext db) { _db = db; }

        [HttpGet("")]
        public async Task<IActionResult> Index() => View(await _db.Motors.AsNoTracking().ToListAsync());

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([FromForm] Motor m)
        {
            if (m == null) return BadRequest();
            if (!ModelState.IsValid) return View("Create", m);
            _db.Motors.Add(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var m = await _db.Motors.FindAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, [FromForm] Motor updated)
        {
            if (updated == null || id != updated.Id) return BadRequest();
            if (!ModelState.IsValid) return View("Edit", updated);
            var m = await _db.Motors.FindAsync(id);
            if (m == null) return NotFound();
            m.Name = updated.Name; m.Brand = updated.Brand;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Motors.FindAsync(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var m = await _db.Motors.FindAsync(id);
            if (m == null) return NotFound();
            _db.Motors.Remove(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
