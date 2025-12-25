using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("shina-ui")]
    public class ShinaUiController : Controller
    {
        private readonly AppDbContext _db;
        public ShinaUiController(AppDbContext db) { _db = db; }

        [HttpGet("")]
        public async Task<IActionResult> Index() => View(await _db.Shinas.AsNoTracking().ToListAsync());

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([FromForm] Shina item)
        {
            if (item == null) return BadRequest();
            if (!ModelState.IsValid) return View("Create", item);
            _db.Shinas.Add(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Shinas.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, [FromForm] Shina updated)
        {
            if (updated == null || id != updated.Id) return BadRequest();
            if (!ModelState.IsValid) return View("Edit", updated);
            var item = await _db.Shinas.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = updated.Name; item.Brand = updated.Brand;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Shinas.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var item = await _db.Shinas.FindAsync(id);
            if (item == null) return NotFound();
            _db.Shinas.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
