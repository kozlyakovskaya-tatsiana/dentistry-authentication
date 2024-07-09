using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User?> GetUserAsync(string phoneNumber);
        Task<IEnumerable<User>> GetUsersWithRolesAsync();
        Task<bool> Exists(string phoneNumber);
    }
}
