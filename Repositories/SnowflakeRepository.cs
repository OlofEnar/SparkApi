using Dapper;
using Serilog;
using Snowflake.Data.Client;
using SparkApi.Data;
using SparkApi.Models.DbModels;
using System.Data.Common;

namespace SparkApi.Repositories
{
    public class SnowflakeRepository
    {
        private readonly SnowflakeDbConnection _conn;
        private readonly ApiDbContext _context;

        public SnowflakeRepository(SnowflakeDbConnection conn, ApiDbContext context)
        {
            _conn = conn;
            _context = context;
        }

        public async Task GetSnowflakeDataAsync()
        {
            try
            {
                Console.WriteLine("Connecting to Snowflake...");
                await _conn.OpenAsync(CancellationToken.None).ConfigureAwait(false);
                Console.WriteLine("Connected.");

                using SnowflakeDbCommand cmd = (SnowflakeDbCommand)_conn.CreateCommand();

                cmd.CommandText = "SELECT event_timestamp as \"Event date\"," +
                    "\r\nevent_name as \"Event name\"," +
                    "\r\nEVENT_JSON:userCountry::STRING as \"Country\"," +
                    "\r\nEVENT_JSON:clientVersion::STRING as \"Client version\"," +
                    "\r\nEVENT_JSON:client_id::STRING as \"User\"," +
                    "\r\nFROM ACCOUNT_EVENTS\r\n" +
                    "\r\nWHERE event_timestamp > current_date - 1 and" +
                    "\r\nEVENT_JSON:platform::STRING ='PC_CLIENT' and" +
                    "\r\nEVENT_JSON:client_id::STRING != ''";

                Console.WriteLine("Sending query...");
                var queryId = await cmd.ExecuteAsyncInAsyncMode(CancellationToken.None).ConfigureAwait(false);
                var queryStatus = await cmd.GetQueryStatusAsync(queryId, CancellationToken.None).ConfigureAwait(false);
                using DbDataReader reader = await cmd.GetResultsFromQueryIdAsync(queryId, CancellationToken.None).ConfigureAwait(false);
                Console.WriteLine($"Querystatus: {queryStatus}, query id: {queryId}");

                using var connection = _context.CreateConnection();

                var existingUserIds = new HashSet<string>(await connection.QueryAsync<string>("SELECT id FROM users"));
                var newUserIds = new HashSet<string>();

                var users = new List<User>();
                var events = new List<Event>();

                int dateOrdinal = reader.GetOrdinal("Event date");
                int nameOrdinal = reader.GetOrdinal("Event name");
                int idOrdinal = reader.GetOrdinal("User");
                int countryOrdinal = reader.GetOrdinal("Country");
                int versionOrdinal = reader.GetOrdinal("Client version");

                Console.WriteLine("Extracting user and event data...");
                while (await reader.ReadAsync(CancellationToken.None).ConfigureAwait(false))
                {
                    var userId = reader.GetString(idOrdinal);
                    DateTime date = reader.GetDateTime(dateOrdinal);

                    if (!existingUserIds.Contains(userId) && newUserIds.Add(userId))
                    {
                        var country = reader.IsDBNull(countryOrdinal) ? null : reader.GetString(countryOrdinal);
                        var version = reader.IsDBNull(versionOrdinal) ? null : reader.GetString(versionOrdinal);

                        users.Add(new User
                        {
                            Id = userId,
                            UserCountry = [country],
                            ClientVersion = [version]
                        });
                    }

                    events.Add(new Event
                    {
                        Date = DateTime.SpecifyKind(date, DateTimeKind.Utc),
                        EventName = reader.GetString(nameOrdinal),
                        UserId = userId,
                    });
                }

                Console.WriteLine($"Extracted {users.Count} new users and {events.Count} events.");

                if (users.Count > 0)
                {
                    const string insertUsersSql = @"
                        INSERT INTO users (id, user_country, client_version)
                        VALUES (@Id, @UserCountry, @ClientVersion)";

                    await connection.ExecuteAsync(insertUsersSql, users);
                    Console.WriteLine($"Inserted {users.Count} new users.");
                }

                if (events.Count > 0)
                {
                    const string insertEventsSql = @"
                        INSERT INTO events (date, event_name, user_id)
                        VALUES (@Date, @EventName, @UserId)";

                    await connection.ExecuteAsync(insertEventsSql, events);
                    Console.WriteLine($"Inserted {events.Count} events.");
                } 
        }
            catch (SnowflakeDbException ex)
            {
                Log.Error(ex, "Error retrieving data from Snowflake.");
                throw new Exception("An error occurred while retrieving data from Snowflake.", ex);
            }
            finally
            {
                await _conn.CloseAsync();
            }
        }
    }
}
