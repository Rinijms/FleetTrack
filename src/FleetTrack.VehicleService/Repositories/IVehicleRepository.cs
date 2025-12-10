using FleetTrack.VehicleService.Models;
using FleetTrack.VehicleService.Enums;

namespace FleetTrack.VehicleService.Repositories;

public interface IVehicleRepository
{
    Vehicle Add(Vehicle vehicle);
    IEnumerable<Vehicle> GetAll();
    Vehicle? GetByVehicleCode(string vehicleCode);
    Vehicle UpdateStatus(UpdateVehicleStatusDTO updateStatus); // returns true if updated
    IEnumerable<VehicleStatusHistory> GetHistory(string vehicleCode);
}