using FleetTrack.VehicleLocationService.Data;
using FleetTrack.VehicleLocationService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetTrack.VehicleLocationService.Repositories
{
    public class EFLocationRepository : ILocationRepository
    {
        private readonly VehicleLocationDbContext _db;

        public EFLocationRepository(VehicleLocationDbContext db) => _db = db;

        public async Task<VehicleLocation> AddAsync(VehicleLocation location)
        {
            _db.VehicleLocations.Add(location);
            await _db.SaveChangesAsync();
            return location;
        }

        public async Task<IEnumerable<VehicleLocation>> GetAllAsync(int limit = 100)
        {
            return await _db.VehicleLocations
                .OrderByDescending(x => x.TimeStampUtc)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<VehicleLocation?> GetLatestByVehicleCodeAsync(string vehicleCode)
        {
            return await _db.VehicleLocations
                .Where(x => x.VehicleCode == vehicleCode)
                .OrderByDescending(x => x.TimeStampUtc)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<VehicleLocation>> GetHistoryByVehicleCodeAsync(string vehicleCode, int take = 100)
        {
            return await _db.VehicleLocations
                .Where(x => x.VehicleCode == vehicleCode)
                .OrderByDescending(x => x.TimeStampUtc)
                .Take(take)
                .ToListAsync();
        }

        
    }
}