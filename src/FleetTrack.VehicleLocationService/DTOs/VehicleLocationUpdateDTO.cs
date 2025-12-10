using System.ComponentModel.DataAnnotations;
namespace FleetTrack.VehicleLocationService.DTOs
{
    public class VehicleLocationUpdateDTO
    {
        // Either vehicle code or vehicle id (prefer vehicleCode for humans)
        [Required]
        public string VehicleCode { get; set; } = string.Empty;

        [Range(-90.0, 90.0)]
        public double Latitude { get; set; }

        [Range(-180.0, 180.0)]
        public double Longitude { get; set; }

        // optional, if sender has its own timestamp
        public DateTime? TimestampUtc { get; set; }
    }
}
