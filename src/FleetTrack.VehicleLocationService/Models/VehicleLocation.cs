namespace FleetTrack.VehicleLocationService.Models
{
    public class VehicleLocation
    {
        public int Id { get; set; } //PK   
        public string VehicleCode { get; set; } = string.Empty; // e.g. TRK-0001 
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime TimeStampUtc{ get; set; }  = DateTime.UtcNow;              
    }
}