using FleetTrack.TripService.Models;

namespace FleetTrack.TripService.Repositories
{
    public interface ITripRepository
    {
        IEnumerable<Trip> GetAll();
        Trip? GetByTripCode(string tripcode);
        Trip Add(Trip trip);
        Trip? AssignDriver(string tripCode, string driverCode);
        Trip? Update(Trip trip);
    }
}