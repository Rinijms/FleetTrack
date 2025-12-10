using FleetTrack.TripService.Models;

namespace FleetTrack.TripService.Services
{
   public interface IVehicleClient
    {
        Task<VehicleDTO?> GetVehicleAsync(string vehicleCode);
        Task<bool> UpdateVehicleStatusAsync(string vehicleCode, int status);
    }

}