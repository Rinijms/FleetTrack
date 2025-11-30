using FleetTrack.DriverService.Models;

namespace FleetTrack.DriverService.Repositories
{
    public class InMemoryDriverRepository
    {
        private readonly List<Driver> _drivers = new();
        private int _sequence = 1;

        public Driver Add(Driver driver)
        {
            driver.Status ="Active";
            //driver.Id = Guid.NewGuid(); 
            driver.DriverCode = $"DRV-{_sequence:D4}";
            _sequence++;

            _drivers.Add(driver);
            return driver;
        }

        public IEnumerable<Driver> GetAll() => _drivers;

        public Driver? GetByDriverCode(string code)
        {
            return _drivers.FirstOrDefault(d => d.DriverCode == code);
        }
    }
}
