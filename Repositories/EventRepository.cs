using AutoMapper;
using Dapper;
using SparkApi.Data;
using SparkApi.Models.DbModels;
using SparkApi.Models.DTOs;
using SparkApi.Repositories.Interfaces;

namespace SparkApi.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public EventRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            var sql = $"SELECT * FROM Events WHERE id = @Id";

            using var connection = _context.CreateConnection();
            connection.Open();

            var userEvent = await connection.QuerySingleOrDefaultAsync<Event>(sql, new { id });
                        
            return userEvent;
        }

        public async Task<IEnumerable<Event>> GetEventsByNameAsync(string eventName)
        {
            var sql = $"SELECT * FROM Events WHERE event_name = @EventName";

            using var connection = _context.CreateConnection();
            connection.Open();
            var userEvents = (await connection.QueryAsync<Event>(sql, new { eventName })).ToList();

            return userEvents;
        }

        public async Task<AggregatedResponse> GetAggregatedEventsGroupedByDateAsync(DateOnly startDate, DateOnly endDate)
        {
            var sql = @"
                SELECT 
                    event_date AS ""EventDate"",
                    SUM(event_count) AS ""EventTotal""
                FROM events
                WHERE event_date BETWEEN @StartDate AND @EndDate
                GROUP BY event_date
                ORDER BY event_date;
            ";

            using var connection = _context.CreateConnection();
            connection.Open();

            var aggregatedEvents = (await connection.QueryAsync<AggregatedEventData>(sql, new { startDate, endDate })).ToList();

            var totalEvents = aggregatedEvents.Sum(e => e.EventTotal);

            var response = new AggregatedResponse
            {
                TotalEvents = totalEvents,
                AggregatedEvents = aggregatedEvents
            };

            Utils.Utils.CalcEventDistribution(response);
            return response;
        }

        public async Task<AggregatedResponse> GetAggregatedEventsGroupedByNameAsync(DateOnly startDate, DateOnly endDate)
        {
            var sql = @"
                SELECT 
                    event_name AS ""EventName"",
                    SUM(event_count) AS ""EventTotal""
                FROM events
                WHERE event_date BETWEEN @StartDate AND @EndDate
                GROUP BY event_name
                ORDER BY event_name;
            ";

            using var connection = _context.CreateConnection();
            connection.Open();

            var aggregatedEvents = (await connection.QueryAsync<AggregatedEventData>(sql, new { startDate, endDate })).ToList();

            var totalEvents = aggregatedEvents.Sum(e => e.EventTotal);

            var response = new AggregatedResponse
            {
                TotalEvents = totalEvents,
                AggregatedEvents = aggregatedEvents
            };

            Utils.Utils.CalcEventDistribution(response);
            return response;
        }

        public async Task<AggregatedResponse> GetDailyAggregatedEventsForEventAsync(DateOnly startDate, DateOnly endDate, string eventName)
        {
            var sql = @"
                SELECT 
                    event_date AS ""EventDate"",
                    event_name AS ""EventName"",
                    SUM(event_count) AS ""EventTotal"",
                    COUNT(DISTINCT user_id) AS ""UniqueUsers""
                FROM events
                WHERE event_date BETWEEN @StartDate AND @EndDate
                  AND event_name = @EventName
                GROUP BY event_date, event_name
                ORDER BY event_date;
            ";

            using var connection = _context.CreateConnection();
            connection.Open();

            var aggregatedEvents = (await connection.QueryAsync<AggregatedEventData>(sql, new { startDate, endDate, eventName })).ToList();

            var totalEvents = aggregatedEvents.Sum(e => e.EventTotal);

            var response = new AggregatedResponse
            {
                TotalEvents = totalEvents,
                AggregatedEvents = aggregatedEvents
            };

            Utils.Utils.CalcEventDistribution(response);
            return response;
        }

        public async Task<IEnumerable<AggregatedEventDetail>> GetAggregatedTimestampsByHourAsync(DateOnly startDate, DateOnly endDate, string eventName)
        {
            var sql = @"
                SELECT 
                    to_char((t.""Timestamp"")::timestamp, 'HH24') AS hour,
                    COUNT(*) AS event_count
                FROM 
                    events e,
                    jsonb_to_recordset(e.event_details) AS t(""Timestamp"" text)
                WHERE 
                    (t.""Timestamp"")::timestamp BETWEEN @StartDate AND @EndDate
                    AND e.event_name = @EventName
                GROUP BY 
                    to_char((t.""Timestamp"")::timestamp, 'HH24')
                ORDER BY 
                    hour;
            ";

            using var connection = _context.CreateConnection();
            connection.Open();

            var timestamps = (await connection.QueryAsync<AggregatedEventDetail>(
                sql, new { startDate, endDate, eventName }))
                .ToList();

        return timestamps;
        }
    }
}