using alert_monitoring.Models;
using Prometheus;
using System.Diagnostics.Metrics;

namespace alert_monitoring.Services
{
    public class MetricsService
    {
        private readonly Counter _alertCounter;
        private readonly Gauge _systemResourceGauge;

        public MetricsService()
        {
            _alertCounter = Metrics.CreateCounter("alerts_total",
                "Total number of alerts",
                new CounterConfiguration
                {
                    LabelNames = new[] { "severity", "service" }
                });

            _systemResourceGauge = Metrics.CreateGauge("system_resources",
                "System resource utilization",
                new GaugeConfiguration
                {
                    LabelNames = new[] { "resource_type" }
                });
        }

        public void RecordAlert(AlertSeverity severity, string serviceName)
        {
            _alertCounter
                .WithLabels(severity.ToString(), serviceName)
                .Inc();
        }

        public void UpdateSystemMetrics(double cpuUsage, double memoryUsage)
        {
            _systemResourceGauge
                .WithLabels("cpu")
                .Set(cpuUsage);

            _systemResourceGauge
                .WithLabels("memory")
                .Set(memoryUsage);
        }
    }
}
