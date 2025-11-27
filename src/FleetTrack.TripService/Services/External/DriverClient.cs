
public class DriverClient : IDriverClient
{
    private readonly HttpClient _http;

    public DriverClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<DriverDto?> GetDriverAsync(string driverCode)
    {
        var response = await _http.GetAsync($"/drivers/{driverCode}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<DriverDto>();
    }

    public async Task<bool> UpdateDriverStatusAsync(string driverCode, string newStatus)
    {
        var body = new { status = newStatus };
        var response = await _http.PatchAsJsonAsync($"/drivers/{driverCode}/status", body);
        return response.IsSuccessStatusCode;
    }
}