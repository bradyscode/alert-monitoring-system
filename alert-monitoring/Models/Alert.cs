namespace alert_monitoring.Models
{
    public class Alert
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public AlertSeverity Severity { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string AdditionalDetails { get; set; }
    }
}
