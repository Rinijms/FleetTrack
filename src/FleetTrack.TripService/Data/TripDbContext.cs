
using FleetTrack.TripService.Models;
using Microsoft.EntityFrameworkCore;


namespace FleetTrack.TripService.Data
{
    public class TripDbContext : DbContext
    {
        public TripDbContext (DbContextOptions<TripDbContext> options) : base(options)
        {}
        public required DbSet<Trip> Trips { get; set; }
    }
}