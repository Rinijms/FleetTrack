using FleetTrack.TripService.Models;

namespace FleetTrack.TripService.Services
{
    public class VehicleClient : IVehicleClient
    {
        private readonly HttpClient _http;

        public VehicleClient(HttpClient http)
        {
            _http = http;
        }
            
        public async Task<VehicleDTO?> GetVehicleAsync(string vehicleCode)
        {
            var response = await _http.GetAsync($"/api/vehicle/{vehicleCode}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<VehicleDTO>();
        }

        public async Task<bool> UpdateVehicleStatusAsync(string vehicleCode, int newStatus)
        {
            var body = new { VehicleCode =vehicleCode, Status = newStatus };
            var response = await _http.PutAsJsonAsync($"/api/vehicle/UpdateStatus", body);
            return response.IsSuccessStatusCode;
        }


    }

}