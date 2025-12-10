using FleetTrack.VehicleLocationService.DTOs;
using FleetTrack.VehicleLocationService.Models;
using FleetTrack.VehicleLocationService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetTrack.VehicleLocationService.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repo;

        public LocationService(ILocationRepository repo) => _repo = repo;

        public async Task<VehicleLocation> SaveLocationAsync(VehicleLocationUpdateDTO dto)
        {
            var location = new VehicleLocation
            {
                VehicleCode = dto.VehicleCode,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                TimeStampUtc = dto.TimestampUtc ?? DateTime.UtcNow
            };

            return await _repo.AddAsync(location);
        }

        public Task<IEnumerable<VehicleLocation>> GetHistoryAsync(string vehicleCode, int take = 100)
            => _repo.GetHistoryByVehicleCodeAsync(vehicleCode, take);

        public Task<VehicleLocation?> GetLatestAsync(string vehicleCode)
            => _repo.GetLatestByVehicleCodeAsync(vehicleCode);

        public Task<IEnumerable<VehicleLocation>> GetAllAsync(int limit = 100)
            => _repo.GetAllAsync(limit);
    }
}