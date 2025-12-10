using FleetTrack.VehicleService.Enums;
namespace FleetTrack.VehicleService.Models
{
    public class UpdateVehicleStatusDTO
    {
        public required string VehicleCode { get; set; } 

        public required VehicleStatus Status { get; set; }
         
    }
}