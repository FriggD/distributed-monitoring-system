using MonitorService.Application.DTOs;
using MonitorService.Application.Interfaces;
using MonitorService.Domain.Entities;
using MonitorService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MonitorService.Application.Services;

public class MetricService : IMetricService
{
    private readonly IMetricRepository _repository;
    private readonly IMessagePublisher _publisher;
    private readonly ICacheService _cache;
    private readonly ILogger<MetricService> _logger;

    public MetricService(
        IMetricRepository repository, 
        IMessagePublisher publisher,
        ICacheService cache,
        ILogger<MetricService> logger)
    {
        _repository = repository;
        _publisher = publisher;
        _cache = cache;
        _logger = logger;
    }

    public async Task<MetricDto?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"metric:{id}";
        var cached = await _cache.GetAsync<MetricDto>(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Métrica {Id} retornada do cache", id);
            return cached;
        }

        var metric = await _repository.GetByIdAsync(id);
        if (metric == null) return null;

        var dto = MapToDto(metric);
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
        return dto;
    }

    public async Task<IEnumerable<MetricDto>> GetAllAsync()
    {
        var metrics = await _repository.GetAllAsync();
        return metrics.Select(MapToDto);
    }

    public async Task<IEnumerable<MetricDto>> GetBySourceAsync(string source)
    {
        var metrics = await _repository.GetBySourceAsync(source);
        return metrics.Select(MapToDto);
    }

    public async Task<MetricDto> CreateAsync(CreateMetricDto dto)
    {
        var metric = new Metric
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Value = dto.Value,
            Unit = dto.Unit,
            Source = dto.Source,
            Timestamp = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(metric);
        var result = MapToDto(created);

        try
        {
            await _publisher.PublishAsync("metric.created", new
            {
                Id = result.Id,
                Name = result.Name,
                Value = result.Value,
                Source = result.Source,
                Timestamp = result.Timestamp
            });
            _logger.LogInformation("Evento metric.created publicado para {Id}", result.Id);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao publicar evento no RabbitMQ, continuando sem evento");
        }

        return result;
    }

    private static MetricDto MapToDto(Metric metric)
    {
        return new MetricDto
        {
            Id = metric.Id,
            Name = metric.Name,
            Value = metric.Value,
            Unit = metric.Unit,
            Timestamp = metric.Timestamp,
            Source = metric.Source
        };
    }
}

public interface IMetricService
{
    Task<MetricDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<MetricDto>> GetAllAsync();
    Task<IEnumerable<MetricDto>> GetBySourceAsync(string source);
    Task<MetricDto> CreateAsync(CreateMetricDto dto);
}
