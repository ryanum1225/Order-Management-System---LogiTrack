using Microsoft.EntityFrameworkCore;
using LogiTrack.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LogiTrack.Data
{
    public class LogiTrackContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=logitrack.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
