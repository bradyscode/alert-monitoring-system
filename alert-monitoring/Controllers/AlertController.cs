using alert_monitoring.Models;
using alert_monitoring.Services;
using Microsoft.AspNetCore.Mvc;

namespace alert_monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly AlertService _alertService;

        public AlertController(AlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpPost("raise")]
        public IActionResult RaiseAlert([FromBody] Alert alert)
        {
            if (alert == null)
                return BadRequest("Invalid alert data");

            _alertService.RaiseAlert(
                alert.ServiceName,
                alert.Severity,
                alert.Message,
                alert.AdditionalDetails
            );

            return Ok(new { message = "Alert processed successfully" });
        }

        [HttpGet("recent")]
        public IActionResult GetRecentAlerts([FromQuery] int count = 100)
        {
            var alerts = _alertService.GetRecentAlerts(count);
            return Ok(alerts);
        }
    }
}
