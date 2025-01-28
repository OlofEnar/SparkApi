using Serilog;
using Snowflake.Data.Client;
using SparkApi.Models.DbModels;
using System.Data.Common;

namespace SparkApi.Services
{
    public class SnowflakeService(SnowflakeDbConnection conn, DbService dbService)
    {
        public async Task GetSnowflakeData()
        {
            var existingUserIds = await dbService.GetUserIds();
            var newUserIds = new HashSet<string>();
            var users = new List<User>();
            var events = new List<Event>();
        
            try
            {
                Console.WriteLine("Connecting to Snowflake...");
                await conn.OpenAsync(CancellationToken.None).ConfigureAwait(false);
                Console.WriteLine("Connected.");

                using SnowflakeDbCommand cmd = (SnowflakeDbCommand)conn.CreateCommand();

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
                            UserId = userId,
                            UserCountry = [country],
                            ClientVersion = [version],
                        });
                    }

                    events.Add(new Event
                    {
                        Date = DateTime.SpecifyKind(date, DateTimeKind.Utc),
                        EventName = reader.GetString(nameOrdinal),
                        UserId = userId,
                    });
                }
                Console.WriteLine($"New events: {events.Count}, New users: {users.Count}, users in Db: {existingUserIds.Count}");
            }

            catch (SnowflakeDbException ex)
            {
                Log.Error($"Error retrieving data: {ex.Message}");
            }
            finally
            {
                await conn.CloseAsync();
            }

            const int batchSize = 1000;

            await dbService.ImportNewUserstoDb(users);

            for (int i = 0; i < events.Count; i += batchSize)
            {
                var batch = events.Skip(i).Take(batchSize).ToList();
                await dbService.ImportEventstoDb(batch);
            }
        }
    }
}