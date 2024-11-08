using AutoMapper;
using Serilog;
using SparkApi.Data;

namespace SparkApi.Services
{
    // Runs on startup and imports first all new users from csv,
    // then all event data to db
    public class StartupService(IMapper mapper, IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    Log.Information("Background task is running");
                    var csvService = new CSVService(mapper, dbContext);
                    await csvService.ImportCsvDatatoDb();

                    Log.Information("Updating Activity index");
                    var dbService = new DbService(dbContext);
                    await dbService.UpdateUsersScore();
                    await dbService.UpdateTotalUserEvents();

                    await Task.Delay(TimeSpan.FromMinutes(2000), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unable to import data");           
            }
        }

        private async Task UpdateDatabase(AppDbContext dbContext)
        {
            Log.Information("Background task is running");
            var csvService = new CSVService(mapper, dbContext);
            await csvService.ImportCsvDatatoDb();
        }

        private async Task UpdateActivityIndex(AppDbContext dbContext)
        {
            Log.Information("Updating Activity index");
            var dbService = new DbService(dbContext);
            await dbService.UpdateUsersScore();
        }

    }
}
