using Npgsql;
using System.Data;

namespace SparkApi.Data
{
    public class ApiDbContext
    {
        private readonly IConfiguration _configuration;

        public ApiDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured in the environment.");
            }

            return new NpgsqlConnection(connectionString);
        }
    }
}
