using SparkApi.Data;
using SparkApi.Repositories;
using Dapper;
using Serilog;
namespace SparkApi.Services
{
    public class SnowflakeService
    {
        private readonly ApiDbContext _context;
        private readonly SnowflakeRepository _snowRepo;
        private readonly ImportTimestampService _timestampService;

        public SnowflakeService(ApiDbContext context, SnowflakeRepository snowRepo, ImportTimestampService timestampService)
        {
            _context = context;
            _snowRepo = snowRepo;
            _timestampService = timestampService;
        }

        public async Task ProcessSnowflakeDataAsync()
        {
            Log.Information("Starting ProcessSnowFlakeData...");
            int daysToImport = CalcDaysToImport();
            Log.Information($"Days to fetch from snowflake: {daysToImport}");

            var userSql = GetUserSql();
            var eventSql = GetEventSql();

            using var dbConn = _context.CreateConnection();
            var response = await _snowRepo.GetSnowflakeDataAsync(daysToImport);

            dbConn.Open();
            using var transaction = dbConn.BeginTransaction();
            try
            {
                int userRows = await dbConn.ExecuteAsync(userSql, new { JsonData = response }, transaction);
                int eventRows = await dbConn.ExecuteAsync(eventSql, new { JsonData = response }, transaction);
                transaction.Commit();

                Log.ForContext("SourceContext", "ImportLogger")
                    .Information($"IMPORT SUCCESS, {userRows} new users, {eventRows} events");
                _timestampService.WriteImportTimestamp(DateTime.Now);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during Db import.");
                transaction.Rollback();
            }
            Log.CloseAndFlush();
        }

        private static string GetUserSql()
        {
            string sql = @"
                INSERT INTO users (id, client_version, user_country)
                SELECT DISTINCT 
                    event_data->>'UserId',
                    event_data->>'ClientVersion',
                    event_data->>'Country'
                FROM jsonb_array_elements(@JsonData::jsonb) AS event_data
                ON CONFLICT (id) DO NOTHING;
            ";
            return sql;
        }

        private static string GetEventSql()
        {
            string sql = @"
                INSERT INTO events (date, event_details, event_count, event_name, user_id)
                SELECT 
                    (event_data->>'EventDate')::date,
                    event_data->'EventDetails',
                    (event_data->>'EventCount')::int,
                    event_data->>'EventName',
                    event_data->>'UserId'
                FROM jsonb_array_elements(@JsonData::jsonb) AS event_data;
            ";
            return sql;
        }

        private int CalcDaysToImport()
        {
            DateTime? lastTimestamp = _timestampService.ReadImportTimestamp();
            Log.Information($"Last logged import: {lastTimestamp}");

            int daysToImport = lastTimestamp.HasValue
                ? (int)Math.Ceiling((DateTime.Now - lastTimestamp.Value).TotalDays)
                : 1;
            return daysToImport;
        }
    }
}