using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MockServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MockController : ControllerBase
    {
        private static bool _slowPeriod = true;
        private static readonly object _lock = new();
        private static readonly Random _random = new();

        [HttpPost]
        public async Task<IActionResult> ProcessRequest([FromBody] JsonElement requestPayload)
        {
            // Determine delay
            double delay;
            lock (_lock)
            {
                delay = _slowPeriod ? 5 + _random.NextDouble() * 2 : 0.5 + _random.NextDouble() * 1.5;
            }

            // Simulate processing delay
            await Task.Delay(TimeSpan.FromSeconds(delay));

            // Build response
            var response = new
            {
                status = "success",
                data = $"Processed request: {requestPayload}",
                processing_time = $"{delay:F2} seconds"
            };

            return Ok(response);
        }

        [HttpGet("toggle-slow-period")]
        public IActionResult ToggleSlowPeriod()
        {
            lock (_lock)
            {
                _slowPeriod = !_slowPeriod;
            }

            return Ok(new { slowPeriod = _slowPeriod });
        }
    }
}
