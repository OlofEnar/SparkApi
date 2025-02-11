using SparkApi.Data;
using SparkApi.Repositories;
using Dapper;
namespace SparkApi.Services
{
    public class SnowflakeService
    {
        private readonly ApiDbContext _context;
        private readonly SnowflakeRepository _snowRepo;
        private readonly ILogger<SnowflakeService> _logger;

        public SnowflakeService(ApiDbContext context, SnowflakeRepository snowRepo, ILogger<SnowflakeService> logger)
        {
            _context = context;
            _snowRepo = snowRepo;
            _logger = logger;
        }

        public async Task ProcessSnowflakeDataAsync()
        {
            var userSql = GetUserSql();
            var eventSql = GetEventSql();

            using var dbConn = _context.CreateConnection();
            var response = await _snowRepo.GetSnowflakeDataJsonAsync();

            dbConn.Open();
            using var transaction = dbConn.BeginTransaction();
            try
            {
                await dbConn.ExecuteAsync(userSql, new { JsonData = response }, transaction);
                await dbConn.ExecuteAsync(eventSql, new { JsonData = response }, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting data: " + ex.Message);
                transaction.Rollback();
            }
        }

        private static string GetUserSql()
        {
            string sql = @"
                INSERT INTO users (id, client_version, user_country)
                SELECT DISTINCT 
                    event_data->>'UserId',
                    ARRAY[event_data->>'ClientVersion'],
                    ARRAY[event_data->>'Country']
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
    }
}