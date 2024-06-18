using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        bool CheckPassword(User user, string password);
        Task<IEnumerable<Role>> GetUserRolesAsync(User user);
        Task<User?> FindByPhoneNumberAsync(string phoneNumber, bool includeRefreshTokens = false);
        Task<IEnumerable<User>> GetUsersWithRolesAsync();
    }
}
