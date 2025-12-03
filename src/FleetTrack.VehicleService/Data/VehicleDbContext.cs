using FleetTrack.VehicleService.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetTrack.VehicleService.Data 
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) {}

        public DbSet<Vehicle> Vehicles {get; set;}
        public DbSet<VehicleStatusHistory> VehicleStatusHistory {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleStatusHistory>()
                .HasKey(h => h.Id);

            modelBuilder.Entity<VehicleStatusHistory>()
                .HasOne(h => h.Vehicle)
                .WithMany(v => v.StatusHistory)
                .HasForeignKey(h => h.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}