using Microsoft.EntityFrameworkCore;
using FleetTrack.VehicleLocationService.Models;

namespace FleetTrack.VehicleLocationService.Data
{
    public class VehicleLocationDbContext : DbContext
    {
        public VehicleLocationDbContext(DbContextOptions<VehicleLocationDbContext> options)
            : base(options) { }

        public DbSet<VehicleLocation> VehicleLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleLocation>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.VehicleCode).IsRequired().HasMaxLength(100);
                b.Property(x => x.Latitude);
                b.Property(x => x.Longitude);
                b.Property(x => x.TimeStampUtc).IsRequired();
                b.HasIndex(x => x.VehicleCode);
                b.HasIndex(x => x.TimeStampUtc);
            });
        }
    }
}
