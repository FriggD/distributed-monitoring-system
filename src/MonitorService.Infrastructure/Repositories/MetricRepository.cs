using Microsoft.EntityFrameworkCore;
using MonitorService.Domain.Entities;
using MonitorService.Domain.Interfaces;
using MonitorService.Infrastructure.Data;

namespace MonitorService.Infrastructure.Repositories;

public class MetricRepository : IMetricRepository
{
    private readonly ApplicationDbContext _context;

    public MetricRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Metric?> GetByIdAsync(Guid id)
    {
        return await _context.Metrics.FindAsync(id);
    }

    public async Task<IEnumerable<Metric>> GetAllAsync()
    {
        return await _context.Metrics
            .OrderByDescending(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<Metric>> GetBySourceAsync(string source)
    {
        return await _context.Metrics
            .Where(m => m.Source == source)
            .OrderByDescending(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<Metric> AddAsync(Metric metric)
    {
        await _context.Metrics.AddAsync(metric);
        await _context.SaveChangesAsync();
        return metric;
    }

    public async Task<IEnumerable<Metric>> AddRangeAsync(IEnumerable<Metric> metrics)
    {
        await _context.Metrics.AddRangeAsync(metrics);
        await _context.SaveChangesAsync();
        return metrics;
    }
}
