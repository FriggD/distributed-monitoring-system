using MonitorService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MonitorService.Application.Services;
using MonitorService.API.Middleware;
using MonitorService.API.Filters;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("Application", "MonitorService")
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        IndexFormat = "monitorservice-logs-{0:yyyy.MM.dd}",
        AutoRegisterTemplate = true,
        NumberOfShards = 2,
        NumberOfReplicas = 1
    })
    .CreateLogger();

try
{
    Log.Information("Iniciando MonitorService API");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddAuthorization();

// Configurar PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=monitordb;Username=monitor;Password=monitor123"));

// Registrar Repositories
builder.Services.AddScoped<MonitorService.Domain.Interfaces.IMetricRepository, MonitorService.Infrastructure.Repositories.MetricRepository>();
builder.Services.AddScoped<MonitorService.Domain.Interfaces.IMonitorTargetRepository, MonitorService.Infrastructure.Repositories.MonitorTargetRepository>();

// Registrar Services
builder.Services.AddScoped<IMetricService, MetricService>();

// Registrar RabbitMQ e Redis
builder.Services.AddSingleton<MonitorService.Application.Interfaces.IMessagePublisher, MonitorService.Infrastructure.Messaging.RabbitMqPublisher>();
builder.Services.AddSingleton<MonitorService.Application.Interfaces.ICacheService, MonitorService.Infrastructure.Cache.RedisCacheService>();

// Registrar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<MonitorService.Application.Validators.CreateMetricValidator>();

var app = builder.Build();

// Middleware de tratamento de erros
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Criar banco e tabelas automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação falhou ao iniciar");
}
finally
{
    Log.CloseAndFlush();
}
