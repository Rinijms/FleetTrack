using System.Text.Json.Serialization;
using FleetTrack.VehicleService.Enums;

namespace FleetTrack.VehicleService.Models
{
    public class Vehicle
    {
        public int Id { get; set; }          // PK
        public required string VehicleCode { get; set; } = string.Empty; // external short code, e.g., VH-0001
        public required string RegistrationNumber { get; set; } = string.Empty;
        public required string Type { get; set; } = string.Empty;       // e.g., Truck, Van, Car
        public required VehicleStatus Status { get; set; } = VehicleStatus.Active;        // Active, InService, Maintenance

        [JsonIgnore]
        public List<VehicleStatusHistory>? StatusHistory { get; set; }
    }
}