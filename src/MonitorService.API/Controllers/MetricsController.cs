using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MonitorService.Application.DTOs;
using MonitorService.Application.Services;

namespace MonitorService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MetricsController : ControllerBase
{
    private readonly IMetricService _service;
    private readonly ILogger<MetricsController> _logger;

    public MetricsController(IMetricService service, ILogger<MetricsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Buscando todas as métricas");
        var metrics = await _service.GetAllAsync();
        _logger.LogInformation("Retornadas {Count} métricas", metrics.Count());
        return Ok(metrics);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var metric = await _service.GetByIdAsync(id);
        if (metric == null)
            return NotFound(new { message = "Métrica não encontrada" });
        return Ok(metric);
    }

    [HttpGet("source/{source}")]
    public async Task<IActionResult> GetBySource(string source)
    {
        var metrics = await _service.GetBySourceAsync(source);
        return Ok(metrics);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMetricDto dto)
    {
        _logger.LogInformation("Criando métrica: {Name} de {Source}", dto.Name, dto.Source);
        var created = await _service.CreateAsync(dto);
        _logger.LogInformation("Métrica criada com ID: {Id}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
