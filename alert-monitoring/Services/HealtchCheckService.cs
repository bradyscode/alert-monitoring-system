using alert_monitoring.Models;
using Hardware.Info;
using System.Diagnostics;

namespace alert_monitoring.Services
{
    public class HealthCheckService : BackgroundService
    {
        private readonly ILogger<HealthCheckService> _logger;
        private readonly AlertService _alertService;
        private readonly MetricsService _metricsService;
        public HealthCheckService(
            ILogger<HealthCheckService> logger,
            AlertService alertService, MetricsService metricsService)
        {
            _logger = logger;
            _alertService = alertService;
            _metricsService = metricsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var cpuUsage = GetCpuUsage();
                var memoryUsage = GetMemoryUsage();

                _metricsService.UpdateSystemMetrics(cpuUsage, memoryUsage);

                await PerformSystemHealthCheck();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task PerformSystemHealthCheck()
        {
            try
            {
                // Simulated health checks
                var cpuUsage = GetCpuUsage();
                var memoryUsage = GetMemoryUsage();

                if (cpuUsage > 90)
                {
                    _alertService.RaiseAlert(
                        "SystemMonitor",
                        AlertSeverity.Critical,
                        $"High CPU Usage: {cpuUsage}%",
                        "Potential performance bottleneck detected"
                    );
                }

                if (memoryUsage > 85)
                {
                    _alertService.RaiseAlert(
                        "SystemMonitor",
                        AlertSeverity.Warning,
                        $"High Memory Usage: {memoryUsage}%",
                        "Memory consumption approaching limit"
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Health check failed: {ex.Message}");
            }
        }

        // Simulated methods - replace with actual system monitoring logic
        private double GetCpuUsage()
        {
            PerformanceCounter cpuCounter;

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return cpuCounter.NextValue();
        }
        private double GetMemoryUsage()
        {
            var memory = 0.0;
            using (Process proc = Process.GetCurrentProcess())
            {
                memory = proc.PrivateMemorySize64 / (1024 * 1024);
            }
            return memory;

        }
    }
}
