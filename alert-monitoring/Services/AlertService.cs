using alert_monitoring.Models;
using System.Collections.Concurrent;

namespace alert_monitoring.Services
{
    public class AlertService
    {
        private readonly ConcurrentQueue<Alert> _alertQueue;
        private readonly ILogger<AlertService> _logger;

        public AlertService(ILogger<AlertService> logger)
        {
            _alertQueue = new ConcurrentQueue<Alert>();
            _logger = logger;
        }
        public void RaiseAlert(string serviceName, AlertSeverity severity, string message, string additionalDetails = null)
        {
            var alert = new Alert
            {
                Id = Guid.NewGuid(),
                ServiceName = serviceName,
                Severity = severity,
                Message = message,
                Timestamp = DateTime.UtcNow,
                AdditionalDetails = additionalDetails
            };

            _alertQueue.Enqueue(alert);
            ProcessAlert(alert);
        }

        private void ProcessAlert(Alert alert)
        {
            switch (alert.Severity)
            {
                case AlertSeverity.Critical:
                    _logger.LogCritical($"CRITICAL ALERT: {alert.ServiceName} - {alert.Message}");
                    // Here you could add additional logic like sending notifications
                    break;
                case AlertSeverity.Error:
                    _logger.LogError($"ERROR ALERT: {alert.ServiceName} - {alert.Message}");
                    break;
                case AlertSeverity.Warning:
                    _logger.LogWarning($"WARNING ALERT: {alert.ServiceName} - {alert.Message}");
                    break;
                default:
                    _logger.LogInformation($"INFO ALERT: {alert.ServiceName} - {alert.Message}");
                    break;
            }
        }

        public IEnumerable<Alert> GetRecentAlerts(int count = 100)
        {
            return _alertQueue.Take(count);
        }
    }
}
