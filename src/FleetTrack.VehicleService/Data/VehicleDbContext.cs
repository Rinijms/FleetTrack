using FleetTrack.VehicleService.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetTrack.VehicleService.Data 
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) {}

        public DbSet<Vehicle> Vehicles {get; set;}
    }
}