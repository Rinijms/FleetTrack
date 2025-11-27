
public interface IDriverClient
{
    Task<DriverDto?> GetDriverAsync(string driverCode);
    Task<bool> UpdateDriverStatusAsync(string driverCode, string newStatus);
}