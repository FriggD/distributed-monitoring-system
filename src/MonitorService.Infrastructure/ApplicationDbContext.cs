using Microsoft.EntityFrameworkCore;
using MonitorService.Domain.Entities;

namespace MonitorService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Metric> Metrics { get; set; }
    public DbSet<MonitorTarget> MonitorTargets { get; set; }
    public DbSet<AlertRule> AlertRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Metric>(entity =>
        {
            entity.ToTable("Metrics");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.Source).HasMaxLength(100);
        });

        modelBuilder.Entity<MonitorTarget>(entity =>
        {
            entity.ToTable("MonitorTargets");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Endpoint).HasMaxLength(500);
        });

        modelBuilder.Entity<AlertRule>(entity =>
        {
            entity.ToTable("AlertRules");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MetricName).HasMaxLength(100);
            entity.Property(e => e.Condition).HasMaxLength(200);
            entity.Property(e => e.Severity).HasMaxLength(50);
        });
    }
}
