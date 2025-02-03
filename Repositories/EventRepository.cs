using AutoMapper;
using Dapper;
using SparkApi.Data;
using SparkApi.Models.DbModels;
using SparkApi.Models.DTOs.ResponseDTO;
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

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            var sql = "SELECT * FROM events";

            using var connection = _context.CreateConnection();
            connection.Open();
            var userEvents = await connection.QueryAsync<Event>(sql);

            return userEvents;
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

        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(string userId)
        {
            const string sql = "SELECT * FROM Events WHERE user_id = @UserId";

            using var connection = _context.CreateConnection();            
            connection.Open();
            var userEvents = (await connection.QueryAsync<Event>(sql, new { UserId = userId })).ToList();

            return userEvents;
        }
    }
}
