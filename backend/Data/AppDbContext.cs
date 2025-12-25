using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Part> Parts { get; set; } = null!;
        public DbSet<Motor> Motors { get; set; } = null!;
        public DbSet<Shina> Shinas { get; set; } = null!;
        public DbSet<Bolt> Bolts { get; set; } = null!;
    }
}
