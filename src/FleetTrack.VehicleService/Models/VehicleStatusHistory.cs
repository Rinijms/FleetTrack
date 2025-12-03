using FleetTrack.VehicleService.Enums;
using System.Text.Json.Serialization;
namespace FleetTrack.VehicleService.Models
{
    public class VehicleStatusHistory
    {
        public int Id { get; set; }               // Primary Key
        public int VehicleId { get; set; }        // Foreign Key to Vehicle
        public VehicleStatus OldStatus { get; set; }
        public VehicleStatus NewStatus { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public Vehicle? Vehicle { get; set; }
    }
}