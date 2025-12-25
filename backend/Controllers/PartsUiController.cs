using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("parts-ui")]
    public class PartsUiController : Controller
    {
        private readonly AppDbContext _db;

        public PartsUiController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var parts = await _db.Parts.AsNoTracking().ToListAsync();
            return View(parts);
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([FromForm] Part part)
        {
            if (part == null) return BadRequest();
            if (!ModelState.IsValid) return View("Create", part);
            _db.Parts.Add(part);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();
            return View(part);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, [FromForm] Part updated)
        {
            if (updated == null || id != updated.Id) return BadRequest();
            if (!ModelState.IsValid) return View("Edit", updated);
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();
            part.Name = updated.Name;
            part.Category = updated.Category;
            part.Price = updated.Price;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();
            return View(part);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();
            _db.Parts.Remove(part);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
