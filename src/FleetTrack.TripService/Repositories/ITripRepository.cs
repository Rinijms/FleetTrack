using FleetTrack.TripService.Models;

namespace FleetTrack.TripService.Repositories
{
    public interface ITripRepository
    {
        IEnumerable<Trip> GetAll();
        Trip? GetByTripCode(string tripcode);
        Trip Add(Trip trip);
        bool AssignDriver(string tripCode, string driverCode);
        public Trip? Update(Trip trip);
    }
}