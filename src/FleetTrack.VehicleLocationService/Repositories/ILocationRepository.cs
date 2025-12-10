using FleetTrack.VehicleLocationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetTrack.VehicleLocationService.Repositories;

public interface ILocationRepository
{
    Task<VehicleLocation> AddAsync(VehicleLocation location);
    Task<IEnumerable<VehicleLocation>> GetAllAsync(int limit = 100);
    Task<VehicleLocation?> GetLatestByVehicleCodeAsync(string vehicleCode);
    Task<IEnumerable<VehicleLocation>> GetHistoryByVehicleCodeAsync(string vehicleCode, int take = 100);
}