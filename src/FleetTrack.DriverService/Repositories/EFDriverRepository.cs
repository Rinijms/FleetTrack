using FleetTrack.DriverService.Models;
using Microsoft.EntityFrameworkCore;
using FleetTrack.DriverService.Data;

namespace FleetTrack.DriverService.Repositories
{
    public class EFDriverRepository : IDriverRepository
    {
        private readonly DriverDbContext _db;

        public EFDriverRepository(DriverDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Driver> GetAll()
        {
            return _db.Drivers.ToList();
        }
        public Driver? GetByDriverCode(string code)
        {
            return _db.Drivers.FirstOrDefault(t => t.DriverCode== code);
        }
         
        public Driver Add(Driver driver)
        {
            driver.Status = "Active";    
            _db.Drivers.Add(driver);
            _db.SaveChanges();
            
            driver.DriverCode = $"DRV-{driver.Id:D4}";    
            _db.Drivers.Update(driver);
            _db.SaveChanges();

            return driver;
        }
    }
}