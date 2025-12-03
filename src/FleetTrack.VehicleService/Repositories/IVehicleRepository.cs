using FleetTrack.VehicleService.Models;
using FleetTrack.VehicleService.Enums;

namespace FleetTrack.VehicleService.Repositories;

public interface IVehicleRepository
{
    Vehicle Add(Vehicle vehicle);
    IEnumerable<Vehicle> GetAll();
    Vehicle? GetByVehicleCode(string vehicleCode);
    bool UpdateStatus(string vehicleCode, VehicleStatus newStatus); // returns true if updated
    IEnumerable<VehicleStatusHistory> GetHistory(string vehicleCode);
}