using FleetTrack.TripService.Models;

namespace FleetTrack.TripService.Repositories
{
    public class InMemoryTripRepository : ITripRepository
    {
        private readonly List<Trip> _trips = new();

        public IEnumerable<Trip> GetAll() => _trips;

        public Trip? GetByTripCode(string tripcode) =>
            _trips.FirstOrDefault(t => t.TripCode.Equals(tripcode,StringComparison.OrdinalIgnoreCase));

        public Trip Add(Trip trip)
        {
            trip.TripId= Guid.NewGuid(); //generates new guid id for new trip
            trip.TripCode = GenerateTripCode(); //user friendly trip code for searching

            _trips.Add(trip);
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
        public bool AssignDriver(string tripCode, string driverCode)
        {
            var trip = _trips.FirstOrDefault(t => t.TripCode == tripCode);
            if (trip == null)
                return false;

            trip.DriverCode = driverCode;
            trip.Status = "DriverAssigned";
            return true;
        } 
        public Trip? Update(Trip trip)
        {
            var existing = _trips.FirstOrDefault(t => t.TripCode == trip.TripCode);
            if (existing == null)
                return null;

            existing.DriverCode = trip.DriverCode;
            existing.StartTime = trip.StartTime;
            existing.EndTime = trip.EndTime;

            return existing;
        }

    }
}
