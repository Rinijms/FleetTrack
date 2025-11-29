using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetTrack.TripService.Models
{
    public class Trip
    {
        [Key]
        public Guid TripId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TripCode { get; set; } = string.Empty;

        // DriverCode is optional (a trip may be unassigned)
        [MaxLength(50)]
        public string? DriverCode { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending";

        // Vehicle is required only if your business rule needs it.
        [MaxLength(50)]
        public string? VehicleCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string StartLocation { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string EndLocation { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
