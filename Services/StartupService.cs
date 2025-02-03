//using AutoMapper;
//using Serilog;
//using SparkApi.Data;

//namespace SparkApi.Services
//{
//    // Runs on startup and imports first all new users from csv,
//    // then all event data to db
//    public class StartupService(IMapper mapper, IServiceProvider serviceProvider) : BackgroundService
//    {
//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            try
//            {
//                while (!stoppingToken.IsCancellationRequested)
//                {
//                    using var scope = serviceProvider.CreateScope();
//                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//                    Log.Information("Background task is running");
//                    //var csvService = new CSVService(mapper, dbContext);
//                    //await csvService.ImportCsvDatatoDb();

//                    Log.Information("Updating Activity index");
//                    var dbService = new DbService(mapper, dbContext);
//                    await dbService.UpdateUsersScoreandTotalEvents();

//                    await Task.Delay(TimeSpan.FromMinutes(2000), stoppingToken);
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "Unable to import data");           
//            }
//        }
//    }
//}
