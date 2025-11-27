using FleetTrack.VehicleService.Models;
namespace FleetTrack.VehicleService.Repositories;

public interface IVehicleRepository
{
    Vehicle Add(Vehicle vehicle);
    IEnumerable<Vehicle> GetAll();
    Vehicle? GetByVehicleCode(string vehicleCode);
    bool UpdateStatus(string vehicleCode, string newStatus); // returns true if updated
}