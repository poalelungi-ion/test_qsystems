using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using In_MethodAPI;

namespace In_MethodAPI
{
    public class MethodController : ControllerBase  // Define the controller class inheriting from ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;
        private static int _roundRobinIndex = 0;
        private static readonly List<string> _mockServers = new List<string>
        {
            "https://localhost:44337/Mock",
            "https://localhost:44338/Mock",
            "https://localhost:44339/Mock"
        };
        private readonly int _serverTimeoutSeconds = 10;

        public MethodController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        // Endpoint for IN_Method1
        [HttpPost("IN_Method1")]
        public async Task<IActionResult> IN_Method1([FromBody] RequestPayload request)
        {
            var component = DetermineComponent(request);
            var response = await CallMockServer(request);
            await LogRequestToDatabase(request, response, "IN_Method1", component);
            return Ok(response);
        }

        // Endpoint for IN_Method2
        [HttpPost("IN_Method2")]
        public async Task<IActionResult> IN_Method2([FromBody] RequestPayload request)
        {
            var component = DetermineComponent(request);
            var response = await CallMockServer(request);
            await LogRequestToDatabase(request, response, "IN_Method2", component);
            return Ok(response);
        }

        // Determines the component based on round robin logic
        private string DetermineComponent(RequestPayload request)
        {
            var component = $"Component{_roundRobinIndex + 1}";
            _roundRobinIndex = (_roundRobinIndex + 1) % _mockServers.Count;
            return component;
        }

        // Calls the mock server and handles retries
        private async Task<string> CallMockServer(RequestPayload request)
        {
            var client = _httpClientFactory.CreateClient();
            string serverUrl = _mockServers[_roundRobinIndex];
            try
            {
                var response = await client.PostAsJsonAsync(serverUrl, request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return await RetryWithNextServer(request);
            }
            catch (Exception)
            {
                return await RetryWithNextServer(request);
            }
        }

        // Retries with the next server if the current one fails
        private async Task<string> RetryWithNextServer(RequestPayload request)
        {
            for (int i = 1; i < _mockServers.Count; i++)
            {
                _roundRobinIndex = (_roundRobinIndex + 1) % _mockServers.Count;
                var serverUrl = _mockServers[_roundRobinIndex];

                try
                {
                    var response = await CallMockServerWithTimeout(request, serverUrl);
                    if (!string.IsNullOrEmpty(response)) return response;
                }
                catch
                {
                    continue;
                }
            }
            return "All mock servers failed to respond.";
        }

        // Calls the mock server with a timeout
        private async Task<string> CallMockServerWithTimeout(RequestPayload request, string serverUrl)
        {
            var client = _httpClientFactory.CreateClient();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_serverTimeoutSeconds));

            try
            {
                var response = await client.PostAsJsonAsync(serverUrl, request, cts.Token);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Logs the request and response to the database
        private async Task LogRequestToDatabase(RequestPayload request, string response, string methodName, string component)
        {
            var logEntry = new RequestLog
            {
                Request = JsonSerializer.Serialize(request),
                Response = response,
                Method = methodName,
                Component = component,
                Timestamp = DateTime.UtcNow
            };

            await _context.RequestLogs.AddAsync(logEntry);
            await _context.SaveChangesAsync();
        }
    }

    // Define the RequestPayload class
    public class RequestPayload
    {
        public string RequestData { get; set; } // Example property, customize as per your needs
    }

    // Define the RequestLog class (this is required for database logging)
    public class RequestLog
    {
        public int Id { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Method { get; set; }
        public string Component { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
