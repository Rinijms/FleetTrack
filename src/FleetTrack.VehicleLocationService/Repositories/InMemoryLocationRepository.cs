using FleetTrack.VehicleLocationService.Models;
using System.Collections.Concurrent;

namespace FleetTrack.VehicleLocationService.Repositories;

public class InMemoryLocationRepository
{
    // simple thread-safe storage using concurrent dictionary with incremental id
    private readonly ConcurrentDictionary<int, VehicleLocation> _store = new();
    private int _idCounter = 0;

    public Task<VehicleLocation> AddAsync(VehicleLocation record, CancellationToken ct = default)
    {
        var id = Interlocked.Increment(ref _idCounter);
        record.Id = id;
        record.TimeStampUtc = record.TimeStampUtc == default ? DateTime.UtcNow : record.TimeStampUtc;
        _store[id] = record;
        return Task.FromResult(record);
    }

    public Task<IReadOnlyList<VehicleLocation>> GetAllAsync(CancellationToken ct = default)
    {
        var list = _store.Values.OrderByDescending(x => x.TimeStampUtc).ToList().AsReadOnly();
        return Task.FromResult((IReadOnlyList<VehicleLocation>)list);
    }

    public Task<IReadOnlyList<VehicleLocation>> GetByVehicleAsync(string vehicleId, CancellationToken ct = default)
    {
        var list = _store.Values
            .Where(x => string.Equals(x.VehicleCode, vehicleId, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.TimeStampUtc)
            .ToList()
            .AsReadOnly();
        return Task.FromResult((IReadOnlyList<VehicleLocation>)list);
    }

    public Task ClearAsync(CancellationToken ct = default)
    {
        _store.Clear();
        Interlocked.Exchange(ref _idCounter, 0);
        return Task.CompletedTask;
    }
}