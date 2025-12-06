using FleetTrack.TripService.Models;
using FleetTrack.TripService.Repositories; 
using FleetTrack.TripService.Enums;

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

    public async Task<Trip> AssignDriverAsync(AssignDriverRequest assignDriver)
    {
        if (string.IsNullOrEmpty(assignDriver.TripCode))
            throw new InvalidOperationException("TripCode is required");

        var trip = _repo.GetByTripCode(assignDriver.TripCode);
        if (trip == null)
            throw new KeyNotFoundException("Trip not found");

        if (!string.IsNullOrEmpty(trip.DriverCode))
            throw new InvalidOperationException("Trip already has a driver assigned");
           
        if (string.IsNullOrEmpty(assignDriver.DriverCode))
            throw new InvalidOperationException("DriverCode is required");

        var driver = await _driverClient.GetDriverAsync(assignDriver.DriverCode);
        if (driver == null)
            throw new KeyNotFoundException("Driver not found");

        if ((DriverStatus)driver.Status != DriverStatus.Active)
            throw new InvalidOperationException("Driver is not available");

        trip.DriverCode = assignDriver.DriverCode;
        trip.Status = TripStatus.DriverAssigned;

        _repo.Update(trip);

        var updated = await _driverClient.UpdateDriverStatusAsync(assignDriver.DriverCode,(int) DriverStatus.Busy);
        if (!updated)
            throw new Exception("Failed to update driver status in DriverService");

        return trip;
    }
}