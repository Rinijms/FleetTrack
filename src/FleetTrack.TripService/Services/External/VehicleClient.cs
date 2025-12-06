namespace FleetTrack.TripService.Services
{
    public class VehicleClient : IVehicleClient
    {
        private readonly HttpClient _http;

        public VehicleClient(HttpClient http)
        {
            _http = http;
        }

    }
}