using FleetTrack.VehicleLocationService.Models;
using System.Collections.Concurrent;

namespace FleetTrack.VehicleLocationService.Repositories;

public class InMemoryLocationRepository : ILocationRepository
{
    // simple thread-safe storage using concurrent dictionary with incremental id
    private readonly ConcurrentDictionary<int, LocationRecord> _store = new();
    private int _idCounter = 0;

    public Task<LocationRecord> AddAsync(LocationRecord record, CancellationToken ct = default)
    {
        var id = Interlocked.Increment(ref _idCounter);
        record.Id = id;
        record.TimestampUtc = record.TimestampUtc == default ? DateTime.UtcNow : record.TimestampUtc;
        _store[id] = record;
        return Task.FromResult(record);
    }

    public Task<IReadOnlyList<LocationRecord>> GetAllAsync(CancellationToken ct = default)
    {
        var list = _store.Values.OrderByDescending(x => x.TimestampUtc).ToList().AsReadOnly();
        return Task.FromResult((IReadOnlyList<LocationRecord>)list);
    }

    public Task<IReadOnlyList<LocationRecord>> GetByVehicleAsync(string vehicleId, CancellationToken ct = default)
    {
        var list = _store.Values
            .Where(x => string.Equals(x.VehicleId, vehicleId, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.TimestampUtc)
            .ToList()
            .AsReadOnly();
        return Task.FromResult((IReadOnlyList<LocationRecord>)list);
    }

    public Task ClearAsync(CancellationToken ct = default)
    {
        _store.Clear();
        Interlocked.Exchange(ref _idCounter, 0);
        return Task.CompletedTask;
    }
}