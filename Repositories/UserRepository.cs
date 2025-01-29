using SparkApi.Data;
using SparkApi.Models.DbModels;
using Dapper;

namespace SparkApi.Repositories
{
    public class UserRepository(ApiDbContext context) : IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllUsersWithEventsAsync()
        {
            var sql = "SELECT * FROM Users";

            using var connection = context.CreateConnection();

            connection.Open();
            var result = await connection.QueryAsync<User>(sql);
            return result.ToList();
        }

        public async Task<IEnumerable<User>> GetAllUsersOnlyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
