using FleetTrack.TripService.Models;

namespace FleetTrack.TripService.Services;


public interface ITripAssignmentService
{
    Task<Trip> AssignDriverAsync(string tripCode, string driverCode);
}
