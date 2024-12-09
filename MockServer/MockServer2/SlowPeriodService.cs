using Microsoft.Extensions.Hosting;

namespace MockServer
{
    public class SlowPeriodService : BackgroundService
    {
        private static readonly object _lock = new();
        private static bool _slowPeriod = true; // Start with slow period enabled

        public static bool SlowPeriod => _slowPeriod;

        public static void ToggleSlowPeriod()
        {
            lock (_lock)
            {
                _slowPeriod = !_slowPeriod; // Toggle the state
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Perform the background task, checking the slow period state
                bool isSlow = _slowPeriod;

                // Simulate slow period logic
                if (isSlow)
                {
                    // Add logic to handle slow period (e.g., delay processing)
                }

                // Delay for a short period before checking again
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
