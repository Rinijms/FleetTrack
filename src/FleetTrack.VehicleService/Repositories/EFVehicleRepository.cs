using FleetTrack.VehicleService.Models;
using FleetTrack.VehicleService.Data;
using FleetTrack.VehicleService.Enums;

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
        vehicle.Status = VehicleStatus.Active;    
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

    public Vehicle UpdateStatus(UpdateVehicleStatusDTO updateDTO)
        {
            
            var vehicle = GetByVehicleCode(updateDTO.VehicleCode);      

            var oldStatus = vehicle.Status;
                
            // Update status
            vehicle.Status = updateDTO.Status;     
            
            // Add history record
            var history = new VehicleStatusHistory
            {
                VehicleId = vehicle.Id,
                OldStatus = oldStatus,
                NewStatus = updateDTO.Status,
                ChangedAt = DateTime.UtcNow
            };

            _db.VehicleStatusHistory.Add(history);
            _db.Vehicles.Update(vehicle);
            _db.SaveChanges();                       

            return vehicle;
        }

    public IEnumerable<VehicleStatusHistory> GetHistory(string vehicleCode)
    {
        var vehicle = GetByVehicleCode(vehicleCode);
        if(vehicle == null)
            return Enumerable.Empty<VehicleStatusHistory>();

        return _db.VehicleStatusHistory
            .Where(h => h.VehicleId == vehicle.Id)
            .OrderByDescending(h => h.ChangedAt)
            .ToList();
    }
}