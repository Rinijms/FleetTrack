namespace FleetTrack.DriverService.Models
{
    public class Driver
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DriverCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        private string _status = "Active";
        public string Status
        {
            get => _status;
            set => _status = value ?? "Active";
        }
    }
}