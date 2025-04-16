using SparkApi.Data;
using SparkApi.Models.DbModels;
using Dapper;
using SparkApi.Repositories.Interfaces;
using AutoMapper;
using SparkApi.Models.DTOs;

namespace SparkApi.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public async Task<IEnumerable<User>> GetUsersWithEventsAsync()
        //{
        //    const string userSql = "SELECT * FROM users";
        //    const string eventSql = "SELECT * FROM events";

        //    using var connection = _context.CreateConnection();
        //    connection.Open();

        //    var users = (await connection.QueryAsync<User>(userSql)).ToList();
        //    var events = (await connection.QueryAsync<Event>(eventSql)).ToList();

        //    var userWithEvents = users.ToDictionary(u => u.Id);

        //    foreach (var userEvent in events)
        //    {
        //        if (userWithEvents.TryGetValue(userEvent.UserId, out var user))
        //        {
        //            user.Events ??= [];
        //            user.Events.Add(userEvent);
        //        }
        //    }
        //    return users;
        //}

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var sql = "SELECT * FROM Users";

            using var connection = _context.CreateConnection();
            connection.Open();
            var users = (await connection.QueryAsync<User>(sql)).ToList();

            return users;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var sql = $"SELECT * FROM users WHERE id = @Id";

            using var connection = _context.CreateConnection();
            connection.Open();
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
            return user;
        }

        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid userId)
        {
            const string sql = "SELECT * FROM Events WHERE user_id = @UserId";

            using var connection = _context.CreateConnection();
            connection.Open();
            var userEvents = (await connection.QueryAsync<Event>(sql, new { UserId = userId })).ToList();

            return userEvents;
        }

        public async Task<AggregatedResponse> GetUsersWithAggregatedEventsAsync(DateOnly startDate, DateOnly endDate)
        {
            const string userSql = "SELECT * FROM users";
            const string eventSql = @"
                SELECT 
                    user_id AS ""UserId"",
                    SUM(event_count) AS ""EventTotal""
                FROM events
                WHERE event_date BETWEEN @StartDate AND @EndDate
                GROUP BY user_id
                ORDER BY SUM(event_count) DESC;
            ";

            using var connection = _context.CreateConnection();
            connection.Open();

            var users = (await connection.QueryAsync<User>(userSql)).ToList();
            var userDict = users.ToDictionary(u => u.Id);

            var aggregatedUsers = (await connection.QueryAsync<AggregatedUserData>(
                eventSql, new { StartDate = startDate, EndDate = endDate }))
                .ToList();

            foreach (var agg in aggregatedUsers)
            {
                if (userDict.TryGetValue(agg.UserId, out var user))
                {
                    agg.User = user;
                }
            }

            var totalEvents = aggregatedUsers.Sum(agg => agg.EventTotal);
            var totalUsers = aggregatedUsers.Count;

            var response = new AggregatedResponse
            {
                TotalUsers = totalUsers,
                TotalEvents = totalEvents,
                AggregatedUsers = aggregatedUsers
            };

            Utils.Utils.CalcEventDistribution(response);

            return response;
        }

        public async Task<AggregatedResponse> GetAggregatedEventsByUserIdGroupedByNameAsync(DateOnly startDate, DateOnly endDate, Guid userId)
        {
            var sql = @"
                SELECT
                    event_name AS ""EventName"",
                    SUM(event_count) AS ""EventTotal""
                FROM events
                WHERE event_date BETWEEN @StartDate AND @EndDate 
                    AND user_id = @UserId 
                GROUP BY event_name
                ORDER BY SUM(event_count) DESC;
            ";

            using var connection = _context.CreateConnection();
            connection.Open();

            var aggregatedEvents = (await connection.QueryAsync<AggregatedEventData>(sql, new { startDate, endDate, userId })).ToList();

            var totalEvents = aggregatedEvents.Sum(e => e.EventTotal);

            var response = new AggregatedResponse
            {
                TotalEvents = totalEvents,
                AggregatedEvents = aggregatedEvents
            };

            Utils.Utils.CalcEventDistribution(response);
            return response;
        }

        public async Task<AggregatedResponse> GetAggregatedEventsByUserIdGroupedByDateAsync(DateOnly startDate, DateOnly endDate, Guid userId)
        {
            var sql = @"
                SELECT
                    event_date AS ""EventDate"",
                    SUM(event_count) AS ""EventTotal""
                FROM events
                WHERE event_date BETWEEN @StartDate AND @EndDate 
                    AND user_id = @UserId 
                GROUP BY event_date
                ORDER BY event_date;
            ";

            using var connection = _context.CreateConnection();
            connection.Open();

            var aggregatedEvents = (await connection.QueryAsync<AggregatedEventData>(sql, new { startDate, endDate, userId })).ToList();

            var totalEvents = aggregatedEvents.Sum(e => e.EventTotal);

            var response = new AggregatedResponse
            {
                TotalEvents = totalEvents,
                AggregatedEvents = aggregatedEvents
            };

            Utils.Utils.CalcEventDistribution(response);
            return response;
        }
    }
}
