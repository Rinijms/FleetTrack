using FleetTrack.VehicleService.Models;
using FleetTrack.VehicleService.Enums;


namespace FleetTrack.VehicleService.Repositories;

public class InMemoryVehicleRepository
{
    private readonly List<Vehicle> _vehicles = new();
    private int _sequence = 1;

    public Vehicle Add(Vehicle vehicle)
    {
        //vehicle.Id = Guid.NewGuid();
        vehicle.VehicleCode = $"VH-{_sequence:D4}";
        _sequence++;
         
        _vehicles.Add(vehicle);
        return vehicle;
    }

    public IEnumerable<Vehicle> GetAll() => _vehicles;

    public Vehicle? GetByVehicleCode(string vehicleCode) =>
        _vehicles.FirstOrDefault(v => v.VehicleCode.Equals(vehicleCode, StringComparison.OrdinalIgnoreCase));

    public bool UpdateStatus(string vehicleCode, VehicleStatus newStatus)
    {
        var v = GetByVehicleCode(vehicleCode);
        if (v == null) return false;
        v.Status = newStatus;
        return true;
    }

    public IEnumerable<VehicleStatusHistory> GetHistory(string vehicleCode)
    {
        return Enumerable.Empty<VehicleStatusHistory>();
    }
}