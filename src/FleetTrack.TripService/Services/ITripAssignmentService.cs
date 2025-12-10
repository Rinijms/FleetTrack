using FleetTrack.TripService.Models;
namespace FleetTrack.TripService.Services
{

    public interface ITripAssignmentService
    {
        Task<Trip> AssignDriverAsync(AssignDriverRequest assignDriver);
        Task<Trip> AssignVehicleAsync(AssignVehicleRequest request);
        Task<Trip?> CompletedTrip(CompleteTripRequest completeTrip);
    }
}