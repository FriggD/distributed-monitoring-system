using MonitorService.Domain.Entities;

namespace MonitorService.Domain.Interfaces;

public interface IMonitorTargetRepository
{
    Task<MonitorTarget?> GetByIdAsync(Guid id);
    Task<IEnumerable<MonitorTarget>> GetAllAsync();
    Task<IEnumerable<MonitorTarget>> GetActiveAsync();
    Task<MonitorTarget> AddAsync(MonitorTarget target);
    Task UpdateAsync(MonitorTarget target);
    Task DeleteAsync(Guid id);
}
