using Microsoft.EntityFrameworkCore;
using LogiTrack.Entities;

namespace LogiTrack.Data
{
    public class LogiTrackContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=logitrack.db");


        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
