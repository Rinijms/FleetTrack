
public class DriverClient : IDriverClient
{
    private readonly HttpClient _http;

    public DriverClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<DriverDto?> GetDriverAsync(string driverCode)
    {
        var response = await _http.GetAsync($"/api/driver/{driverCode}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<DriverDto>();
    }

    public async Task<bool> UpdateDriverStatusAsync(string driverCode, int newStatus)
    {
        var body = new { DriverCode=driverCode, Status = newStatus };
        var response = await _http.PutAsJsonAsync($"/api/driver/updateStatus", body);
        return response.IsSuccessStatusCode;
    }
}