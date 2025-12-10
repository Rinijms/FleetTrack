using System;

namespace FleetTrack.VehicleLocationService.DTOs
{
    public class VehicleLocationDto
    {
        public int Id { get; set; }          
        public string VehicleCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime TimestampUtc { get; set; }
    }
}