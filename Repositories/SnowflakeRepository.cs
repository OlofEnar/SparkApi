using Serilog;
using Snowflake.Data.Client;
using SparkApi.Data;

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

        public async Task<string> GetSnowflakeDataJsonAsync()
        {
            try
            {
                Console.WriteLine("Connecting to Snowflake...");
                await _conn.OpenAsync(CancellationToken.None).ConfigureAwait(false);
                Console.WriteLine("Connected.");

                using var cmd = (SnowflakeDbCommand)_conn.CreateCommand();
                cmd.CommandText = @"
                    WITH BaseEvents AS (
                        SELECT 
                            TO_DATE(event_timestamp) AS EventDate,
                            event_name AS EventName,
                            EVENT_JSON:client_id::STRING AS UserId,
                            EVENT_JSON:userCountry::STRING AS Country,
                            EVENT_JSON:clientVersion::STRING AS ClientVersion,
                            event_timestamp AS Timestamp,
                            COALESCE(NULLIF(EVENT_JSON:view::STRING, ''), 'UNKNOWN') AS FromView
                        FROM ACCOUNT_EVENTS
                        WHERE event_timestamp > CURRENT_DATE - 1
                            AND EVENT_JSON:platform::STRING = 'PC_CLIENT'
                            AND EVENT_JSON:client_id::STRING != ''
                    ),
                    AggregatedEvents AS (
                        SELECT 
                            EventDate,
                            EventName,
                            UserId,
                            Country,
                            ClientVersion,
                            COUNT(*) AS EventCount,
                            ARRAY_AGG(
                                OBJECT_CONSTRUCT(
                                    'Timestamp', Timestamp,
                                    'FromView', FromView
                                )
                            ) AS EventDetails
                        FROM BaseEvents
                        GROUP BY EventDate, EventName, UserId, Country, ClientVersion
                    )
                    SELECT ARRAY_AGG(
                        OBJECT_CONSTRUCT(
                            'EventDate', EventDate,
                            'EventName', EventName,
                            'UserId', UserId,
                            'Country', Country,
                            'ClientVersion', ClientVersion,
                            'EventCount', EventCount,
                            'EventDetails', EventDetails
                        )
                    ) AS json_result
                    FROM AggregatedEvents;
                ";

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string response = reader.GetString(0);
                    return response;
                }
                return "No data to read";
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
