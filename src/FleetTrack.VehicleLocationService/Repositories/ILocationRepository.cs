using FleetTrack.VehicleLocationService.Models;

namespace FleetTrack.VehicleLocationService.Repositories;

public interface ILocationRepository
{
    Task<LocationRecord> AddAsync(LocationRecord record, CancellationToken ct = default);
    Task<IReadOnlyList<LocationRecord>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<LocationRecord>> GetByVehicleAsync(string vehicleId, CancellationToken ct = default);
    Task ClearAsync(CancellationToken ct = default);
}