using FleetTrack.TripService.Models;
using FleetTrack.TripService.Repositories; 

namespace FleetTrack.TripService.Services;

public class TripAssignmentService : ITripAssignmentService
{
    private readonly ITripRepository _repo;
    private readonly IDriverClient _driverClient;

    public TripAssignmentService(ITripRepository repo, IDriverClient driverClient)
    {
        _repo = repo;
        _driverClient = driverClient;
    }

    public async Task<Trip> AssignDriverAsync(string tripCode, string driverCode)
    {
        var trip = _repo.GetByTripCode(tripCode);
        if (trip == null)
            throw new KeyNotFoundException("Trip not found");

        if (!string.IsNullOrEmpty(trip.DriverCode))
            throw new InvalidOperationException("Trip already has a driver assigned");

        var driver = await _driverClient.GetDriverAsync(driverCode);
        if (driver == null)
            throw new KeyNotFoundException("Driver not found");

        if (!driver.Status.Equals("available", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Driver is not available");

        trip.DriverCode = driverCode;
        trip.Status = "Assigned";

        _repo.Update(trip);

        var updated = await _driverClient.UpdateDriverStatusAsync(driverCode, "busy");
        if (!updated)
            throw new Exception("Failed to update driver status in DriverService");

        return trip;
    }
}