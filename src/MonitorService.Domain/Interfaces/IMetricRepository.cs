using MonitorService.Domain.Entities;

namespace MonitorService.Domain.Interfaces;

public interface IMetricRepository
{
    Task<Metric?> GetByIdAsync(Guid id);
    Task<IEnumerable<Metric>> GetAllAsync();
    Task<IEnumerable<Metric>> GetBySourceAsync(string source);
    Task<Metric> AddAsync(Metric metric);
    Task<IEnumerable<Metric>> AddRangeAsync(IEnumerable<Metric> metrics);
}
