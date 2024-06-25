using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User?> GetDetailedInfoByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<User>> GetUsersWithRolesAsync();
        Task<bool> Exists(string phoneNumber);
    }
}
