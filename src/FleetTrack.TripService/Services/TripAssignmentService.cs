using FleetTrack.TripService.Models;
using FleetTrack.TripService.Repositories; 
using FleetTrack.TripService.Enums;

namespace FleetTrack.TripService.Services;

public class TripAssignmentService : ITripAssignmentService
{
    private readonly ITripRepository _repo;
    private readonly IDriverClient _driverClient;
    private readonly IVehicleClient _vehicleClient;

    public TripAssignmentService(ITripRepository repo, IDriverClient driverClient,IVehicleClient vehicleClient)
    {
        _repo = repo;
        _driverClient = driverClient;
        _vehicleClient = vehicleClient;
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

    public async Task<Trip> AssignVehicleAsync(AssignVehicleRequest request)
    {
        if (string.IsNullOrEmpty(request.TripCode))
            throw new InvalidOperationException("TripCode is required");
        if (string.IsNullOrEmpty(request.VehicleCode))
            throw new InvalidOperationException("VehicleCode is required");

        var trip = _repo.GetByTripCode(request.TripCode);
        if (trip == null)
            throw new KeyNotFoundException("Trip not found");

        if (!string.IsNullOrEmpty(trip.VehicleCode))
            throw new InvalidOperationException("Trip already has a vehicle assigned");

        var vehicle = await _vehicleClient.GetVehicleAsync(request.VehicleCode);
        if (vehicle == null)
            throw new KeyNotFoundException("Vehicle not found");

        if ((VehicleStatus)vehicle.Status != VehicleStatus.Active)
            throw new InvalidOperationException("Vehicle is not available");

        trip.VehicleCode = request.VehicleCode;
        trip.Status = TripStatus.VehicleAssigned;

        _repo.Update(trip);

        var updated = await _vehicleClient.UpdateVehicleStatusAsync(
            request.VehicleCode,
            (int)VehicleStatus.Booked);

        if (!updated)
            throw new Exception("Failed to update vehicle status in VehicleService");

        return trip;
    }
}