using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitorService.Domain.Entities
{
    public class AlertRule
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MetricName { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public double Threshold { get; set; }
        public string Severity { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
    }
}