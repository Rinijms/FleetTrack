using FleetTrack.VehicleLocationService.DTOs;
using FleetTrack.VehicleLocationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetTrack.VehicleLocationService.Services
{
    public interface ILocationService
    {
        Task<VehicleLocation> SaveLocationAsync(VehicleLocationUpdateDTO dto);
        Task<IEnumerable<VehicleLocation>> GetHistoryAsync(string vehicleCode, int take = 100);
        Task<VehicleLocation?> GetLatestAsync(string vehicleCode);
        Task<IEnumerable<VehicleLocation>> GetAllAsync(int limit = 100);
    }
}
