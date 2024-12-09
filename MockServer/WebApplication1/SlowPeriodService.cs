using Microsoft.Extensions.Hosting;

namespace MockServer
{
    public class SlowPeriodService : BackgroundService
    {
        private static readonly object _lock = new();
        private static bool _slowPeriod = false;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                lock (_lock)
                {
                    _slowPeriod = !_slowPeriod;
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Toggle every 10 seconds
            }
        }
    }
}
