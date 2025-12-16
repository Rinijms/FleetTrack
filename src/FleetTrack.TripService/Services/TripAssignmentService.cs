using FleetTrack.TripService.Models;
using FleetTrack.TripService.Repositories; 
using FleetTrack.TripService.Enums;
using FleetTrack.Shared.EventBus;
using FleetTrack.Shared.Events;

namespace FleetTrack.TripService.Services;

public class TripAssignmentService : ITripAssignmentService
{
    private readonly ITripRepository _repo;
    private readonly IDriverClient _driverClient;
    private readonly IVehicleClient _vehicleClient;
    private readonly IEventBusProducer _producer;
    public TripAssignmentService(ITripRepository repo, IDriverClient driverClient,IVehicleClient vehicleClient, IEventBusProducer producer)
    {
        _repo = repo;
        _driverClient = driverClient;
        _vehicleClient = vehicleClient;
        _producer= producer;
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

    public async Task<Trip?> CompletedTrip(CompleteTripRequest completeTrip)
    {
        if (string.IsNullOrEmpty(completeTrip.TripCode))
            throw new InvalidOperationException("TripCode is required");
      
        var trip = _repo.GetByTripCode(completeTrip.TripCode);
        if (trip == null)
            throw new KeyNotFoundException("Trip not found");        

        // Validate current state: require at least a driver or vehicle was assigned depending on your business rules.
        // Example: allow completion only if trip is not already completed
        if (trip.Status == TripStatus.Completed)
            throw new InvalidOperationException("Trip is already completed.");

        
        /*BEGIN OF COMMENTING - Vehicle & Driver will be made ACTIVE again via Events

        // If there is an assigned driver — ensure driver exists and set to Active after completion
        if (!string.IsNullOrEmpty(trip.DriverCode))
        {
            var driver = await _driverClient.GetDriverAsync(trip.DriverCode);
            if (driver == null)
                throw new KeyNotFoundException("Assigned driver not found in DriverService.");

            // set driver back to Active
            var driverUpdated = await _driverClient.UpdateDriverStatusAsync(trip.DriverCode, (int)DriverStatus.Active);
            if (!driverUpdated)
                throw new Exception("Failed to update driver status to Active.");
        } 
        
        // If there is an assigned vehicle — ensure vehicle exists and set to Available after completion
        if (!string.IsNullOrEmpty(trip.VehicleCode))
        {
            var vehicle = await _vehicleClient.GetVehicleAsync(trip.VehicleCode);
            if (vehicle == null)
                throw new KeyNotFoundException("Assigned vehicle not found in VehicleService.");

            var vehicleUpdated = await _vehicleClient.UpdateVehicleStatusAsync(trip.VehicleCode, (int)VehicleStatus.Active);
            if (!vehicleUpdated)
                throw new Exception("Failed to update vehicle status to Active.");
        } 
        
        END OF COMMENTING*/
 
        // All external updates succeeded; mark trip completed
        trip.Status = TripStatus.Completed;
        trip.EndTime = DateTime.UtcNow;

        var updatedTrip = _repo.Update(trip); // your existing Update method persists changes

        //Publishing the message to CLOUDAMQP - RabbitMQ
        var eventCompleted =new TripCompletedEvent
        {
            TripCode = trip.TripCode,
            DriverCode = trip.DriverCode?? "",
            VehicleCode = trip.VehicleCode ?? "",
            CompletedAt = trip.EndTime!.Value
        };

        await _producer.PublishAsync(eventCompleted);
        return updatedTrip;
    }


}