using FleetTrack.TripService.Models;
using Microsoft.EntityFrameworkCore;
using FleetTrack.TripService.Data;
using FleetTrack.TripService.Enums;

namespace FleetTrack.TripService.Repositories
{
    public class EFTripRepository : ITripRepository
    {
        private readonly TripDbContext _db;

        public EFTripRepository(TripDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Trip> GetAll()
        {
            return _db.Trips.ToList();
        }

        public Trip? GetByTripCode(string tripCode)
        {
            return _db.Trips.FirstOrDefault(t => t.TripCode==tripCode);
        }

        public Trip Add(Trip trip)
        {
            trip.TripCode = GenerateTripCode();
            _db.Trips.Add(trip);
            _db.SaveChanges();
            return trip;
        }
        private static string GenerateTripCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Range(0, 6)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }
        public Trip? AssignDriver(string tripCode, string driverCode)
        {
            var trip = _db.Trips.FirstOrDefault(t => t.TripCode == tripCode);
            if (trip == null) return null;

            trip.DriverCode = driverCode;
            trip.Status = TripStatus.DriverAssigned;

            _db.SaveChanges();
            return trip;
        }
        public Trip? Update(Trip trip)
        {
            var existing = _db.Trips.FirstOrDefault(t => t.TripCode == trip.TripCode);
            if (existing == null)
                return null;

            existing.DriverCode = trip.DriverCode;
            existing.StartTime = trip.StartTime;
            existing.EndTime = trip.EndTime;

            _db.SaveChanges();
            return existing;
        }

        public Trip? AssignVehicle(string tripCode, string vehicleCode)
        {
            var trip = _db.Trips.FirstOrDefault(t => t.TripCode == tripCode);
            if (trip == null) return null;

            trip.VehicleCode = vehicleCode;
            trip.Status = TripStatus.VehicleAssigned;

            _db.SaveChanges();
            return trip;
        }
    }
    
}
