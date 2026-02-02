using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MonitorService.Application.DTOs;
using MonitorService.Application.Interfaces;
using MonitorService.Application.Services;
using MonitorService.Domain.Entities;
using MonitorService.Domain.Interfaces;

namespace MonitorService.UnitTests.Services;

public class MetricServiceTests
{
    private readonly Mock<IMetricRepository> _mockRepository;
    private readonly Mock<IMessagePublisher> _mockPublisher;
    private readonly Mock<ICacheService> _mockCache;
    private readonly Mock<ILogger<MetricService>> _mockLogger;
    private readonly MetricService _service;

    public MetricServiceTests()
    {
        _mockRepository = new Mock<IMetricRepository>();
        _mockPublisher = new Mock<IMessagePublisher>();
        _mockCache = new Mock<ICacheService>();
        _mockLogger = new Mock<ILogger<MetricService>>();
        _service = new MetricService(_mockRepository.Object, _mockPublisher.Object, _mockCache.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateMetric()
    {
        var dto = new CreateMetricDto
        {
            Name = "CPU Usage",
            Value = 75.5,
            Unit = "%",
            Source = "server-01"
        };

        var createdMetric = new Metric
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Value = dto.Value,
            Unit = dto.Unit,
            Source = dto.Source,
            Timestamp = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Metric>())).ReturnsAsync(createdMetric);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Name.Should().Be(dto.Name);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Metric>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMetrics()
    {
        var metrics = new List<Metric>
        {
            new() { Id = Guid.NewGuid(), Name = "CPU", Value = 75, Unit = "%", Source = "server-01", Timestamp = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Memory", Value = 65, Unit = "%", Source = "server-02", Timestamp = DateTime.UtcNow }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(metrics);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotInCache_ShouldReturnFromRepository()
    {
        var metricId = Guid.NewGuid();
        var metric = new Metric
        {
            Id = metricId,
            Name = "CPU",
            Value = 75,
            Unit = "%",
            Source = "server-01",
            Timestamp = DateTime.UtcNow
        };

        _mockCache.Setup(c => c.GetAsync<MetricDto>(It.IsAny<string>())).ReturnsAsync((MetricDto?)null);
        _mockRepository.Setup(r => r.GetByIdAsync(metricId)).ReturnsAsync(metric);

        var result = await _service.GetByIdAsync(metricId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(metricId);
    }
}
