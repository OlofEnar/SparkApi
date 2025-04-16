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
            var connectionString = Environment.GetEnvironmentVariable("DbConnection");

            return new NpgsqlConnection(connectionString);
        }
    }
}
