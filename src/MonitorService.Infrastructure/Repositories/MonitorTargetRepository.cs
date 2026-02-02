using Microsoft.EntityFrameworkCore;
using MonitorService.Domain.Entities;
using MonitorService.Domain.Interfaces;
using MonitorService.Infrastructure.Data;

namespace MonitorService.Infrastructure.Repositories;

public class MonitorTargetRepository : IMonitorTargetRepository
{
    private readonly ApplicationDbContext _context;

    public MonitorTargetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MonitorTarget?> GetByIdAsync(Guid id)
    {
        return await _context.MonitorTargets.FindAsync(id);
    }

    public async Task<IEnumerable<MonitorTarget>> GetAllAsync()
    {
        return await _context.MonitorTargets.ToListAsync();
    }

    public async Task<IEnumerable<MonitorTarget>> GetActiveAsync()
    {
        return await _context.MonitorTargets
            .Where(t => t.IsActive)
            .ToListAsync();
    }

    public async Task<MonitorTarget> AddAsync(MonitorTarget target)
    {
        await _context.MonitorTargets.AddAsync(target);
        await _context.SaveChangesAsync();
        return target;
    }

    public async Task UpdateAsync(MonitorTarget target)
    {
        _context.MonitorTargets.Update(target);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var target = await GetByIdAsync(id);
        if (target != null)
        {
            _context.MonitorTargets.Remove(target);
            await _context.SaveChangesAsync();
        }
    }
}
