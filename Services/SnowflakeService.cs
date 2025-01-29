using Serilog;
using Snowflake.Data.Client;
using SparkApi.Models.DbModels;
using SparkApi.Repositories;
using System.Data.Common;

namespace SparkApi.Services
{
    public class SnowflakeService(DbService dbService, SnowflakeRepository snowflakeRepo)
    {

        public async Task ProcessSnowflakeDataAsync()
        {
            var reader = await snowflakeRepo.GetSnowflakeDataAsync();
            var existingUserIds = await dbService.GetUserIds();
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
            

            //const int batchSize = 1000;

            //await dbService.ImportNewUserstoDb(users);

            //for (int i = 0; i < events.Count; i += batchSize)
            //{
            //    var batch = events.Skip(i).Take(batchSize).ToList();
            //    await dbService.ImportEventstoDb(batch);
            //}
        }
    }
}