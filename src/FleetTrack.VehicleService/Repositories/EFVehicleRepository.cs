using FleetTrack.VehicleService.Models;
using FleetTrack.VehicleService.Data;

namespace FleetTrack.VehicleService.Repositories;

public class EFVehicleRepository : IVehicleRepository
{
    private readonly VehicleDbContext _db;
    public EFVehicleRepository(VehicleDbContext db)
    {
        _db= db;
    }

    public Vehicle Add(Vehicle vehicle)
    {        
        vehicle.Status = "Active";    
        _db.Vehicles.Add(vehicle);
        _db.SaveChanges();

        vehicle.VehicleCode = $"VH-{vehicle.Id:D4}";    
        _db.Vehicles.Update(vehicle);
        _db.SaveChanges();
             
        return vehicle;
    }

    public IEnumerable<Vehicle> GetAll() => _db.Vehicles.ToList();

    public Vehicle? GetByVehicleCode(string vehicleCode) =>
        _db.Vehicles.FirstOrDefault(v => v.VehicleCode == vehicleCode);

    public bool UpdateStatus(string vehicleCode, string newStatus)
    {
        var v = GetByVehicleCode(vehicleCode);
        if (v == null) return false;
        v.Status = newStatus;

        _db.Vehicles.Update(v);
        _db.SaveChanges();
        return true;
    }
}