using SparkApi.Data;
using SparkApi.Models.DbModels;
using SparkApi.Repositories;
using Dapper;
namespace SparkApi.Services
{
    public class SnowflakeService
    {
        private readonly ApiDbContext _context;
        private readonly SnowflakeRepository _snowflakeRepo;
        private readonly ILogger<SnowflakeService> _logger;


        public SnowflakeService(ApiDbContext context, SnowflakeRepository snowflakeRepo, ILogger<SnowflakeService> logger)
        {
            _context = context;
            _snowflakeRepo = snowflakeRepo;
            _logger = logger;
        }

        //    public async Task ProcessSnowflakeDataAsync()
        //    {
        //        using var connection = _context.CreateConnection();

        //        var reader = await _snowflakeRepo.GetSnowflakeDataAsync();
        //        var existingUserIds = new HashSet<string>(await connection.QueryAsync<string>("SELECT id FROM users"));
        //        var newUserIds = new HashSet<string>();

        //        var users = new List<User>();
        //        var events = new List<Event>();

        //        int dateOrdinal = reader.GetOrdinal("Event date");
        //        int nameOrdinal = reader.GetOrdinal("Event name");
        //        int idOrdinal = reader.GetOrdinal("User");
        //        int countryOrdinal = reader.GetOrdinal("Country");
        //        int versionOrdinal = reader.GetOrdinal("Client version");

        //        Console.WriteLine("Extracting user and event data...");
        //        while (await reader.ReadAsync(CancellationToken.None).ConfigureAwait(false))
        //        {
        //            var userId = reader.GetString(idOrdinal);
        //            DateTime date = reader.GetDateTime(dateOrdinal);

        //            if (!existingUserIds.Contains(userId) && newUserIds.Add(userId))
        //            {
        //                var country = reader.IsDBNull(countryOrdinal) ? null : reader.GetString(countryOrdinal);
        //                var version = reader.IsDBNull(versionOrdinal) ? null : reader.GetString(versionOrdinal);

        //                users.Add(new User
        //                {
        //                    Id = userId,
        //                    UserCountry = [country],
        //                    ClientVersion = [version]
        //                });
        //            }

        //            events.Add(new Event
        //            {
        //                Date = DateTime.SpecifyKind(date, DateTimeKind.Utc),
        //                EventName = reader.GetString(nameOrdinal),
        //                UserId = userId,
        //            });
        //        }

        //        Console.WriteLine($"Extracted {users.Count} new users and {events.Count} events.");

        //        if (users.Count > 0)
        //        {
        //            const string insertUsersSql = @"
        //        INSERT INTO users (id, user_country, client_version)
        //        VALUES (@Id, @UserCountry, @ClientVersion)";

        //            await connection.ExecuteAsync(insertUsersSql, users);
        //            Console.WriteLine($"Inserted {users.Count} new users.");
        //        }

        //        if (events.Count > 0)
        //        {
        //            const string insertEventsSql = @"
        //        INSERT INTO events (date, event_name, user_id)
        //        VALUES (@Date, @EventName, @UserId)";

        //            await connection.ExecuteAsync(insertEventsSql, events);
        //            Console.WriteLine($"Inserted {events.Count} events.");
        //        }
        //    }
        //    //const int batchSize = 1000;

        //    //await dbService.ImportNewUserstoDb(users);

        //    //for (int i = 0; i < events.Count; i += batchSize)
        //    //{
        //    //    var batch = events.Skip(i).Take(batchSize).ToList();
        //    //    await dbService.ImportEventstoDb(batch);
        //    //}
        //}
    }
}
