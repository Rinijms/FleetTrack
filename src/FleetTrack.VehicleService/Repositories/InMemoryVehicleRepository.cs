using FleetTrack.VehicleService.Models;

namespace FleetTrack.VehicleService.Repositories;

public class InMemoryVehicleRepository : IVehicleRepository
{
    private readonly List<Vehicle> _vehicles = new();
    private int _sequence = 1;

    public Vehicle Add(Vehicle vehicle)
    {
        vehicle.Id = Guid.NewGuid();
        vehicle.VehicleCode = $"VH-{_sequence:D4}";
        _sequence++;
        // default status if not provided
        vehicle.Status = string.IsNullOrWhiteSpace(vehicle.Status) ? "Active" : vehicle.Status;

        _vehicles.Add(vehicle);
        return vehicle;
    }

    public IEnumerable<Vehicle> GetAll() => _vehicles;

    public Vehicle? GetByVehicleCode(string vehicleCode) =>
        _vehicles.FirstOrDefault(v => v.VehicleCode.Equals(vehicleCode, StringComparison.OrdinalIgnoreCase));

    public bool UpdateStatus(string vehicleCode, string newStatus)
    {
        var v = GetByVehicleCode(vehicleCode);
        if (v == null) return false;
        v.Status = newStatus;
        return true;
    }
}