using SparkApi.Data;
using SparkApi.Models.DbModels;
using Dapper;
using SparkApi.Repositories.Interfaces;
using AutoMapper;
using SparkApi.Models.DTOs.ResponseDTO;

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

        public async Task<IEnumerable<User>> GetUsersWithEventsAsync()
        {
            const string userSql = "SELECT * FROM users";
            const string eventSql = "SELECT * FROM events";

            using var connection = _context.CreateConnection();
            connection.Open();

            var users = (await connection.QueryAsync<User>(userSql)).ToList();
            var events = (await connection.QueryAsync<Event>(eventSql)).ToList();

            var userWithEvents = users.ToDictionary(u => u.Id);

            foreach (var userEvent in events)
            {
                if (userWithEvents.TryGetValue(userEvent.UserId, out var user))
                {
                    user.Events ??= [];
                    user.Events.Add(userEvent);
                }
            }
            return users;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var sql = "SELECT * FROM Users";

            using var connection = _context.CreateConnection();
            connection.Open();
            var users = (await connection.QueryAsync<User>(sql)).ToList();

            return users;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            var sql = $"SELECT * FROM users WHERE id = @Id";

            using var connection = _context.CreateConnection();
            connection.Open();
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
            return user;
        }
    }
}
