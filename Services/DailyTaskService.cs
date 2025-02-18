using Serilog;

namespace SparkApi.Services
{
    public class DailyTaskService: BackgroundService
    {
        private readonly TimeSpan _scheduledTime = new(9, 53, 0);
        private readonly IServiceProvider _serviceProvider;

        public DailyTaskService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = DateTime.Today.Add(_scheduledTime);
                if (now > nextRun)
                {
                    nextRun = nextRun.AddDays(1);
                }

                var delay = nextRun - now;
                Log.Information($"Waiting {delay} until next import at {nextRun}.");
                await Task.Delay(delay, stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var snowflake = scope.ServiceProvider.GetRequiredService<SnowflakeService>();
                    Log.Information("Starting daily task...");
                    await snowflake.ProcessSnowflakeDataAsync();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error during data import");
                }
            }
        }
    }
}
