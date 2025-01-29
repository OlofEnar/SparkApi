using SparkApi.Models.DbModels;

namespace SparkApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersOnlyAsync();
        Task<IEnumerable<User>> GetAllUsersWithEventsAsync();
    }
}
