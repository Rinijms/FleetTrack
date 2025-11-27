using FleetTrack.DriverService.Models;

namespace FleetTrack.DriverService.Repositories
{
    public interface IDriverRepository
    {
        IEnumerable<Driver> GetAll();
        Driver? GetByDriverCode(string code);
        Driver Add(Driver driver);
    }
}