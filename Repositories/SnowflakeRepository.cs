using Serilog;
using Snowflake.Data.Client;
using System.Data.Common;

namespace SparkApi.Repositories
{
    public class SnowflakeRepository(SnowflakeDbConnection conn)
    {
        public async Task<DbDataReader> GetSnowflakeDataAsync()
        {
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

                return reader;
            }
            catch (SnowflakeDbException ex)
            {
                Log.Error(ex, "Error retrieving data from Snowflake.");
                throw new Exception("An error occurred while retrieving data from Snowflake.", ex);
            }
            finally
            {
                await conn.DisposeAsync();
            }
        }
    }
}
