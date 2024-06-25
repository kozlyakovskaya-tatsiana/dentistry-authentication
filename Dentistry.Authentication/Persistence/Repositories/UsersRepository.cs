using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(DentistryAuthenticationContext context)
            : base(context)
        { }
        public async Task<IEnumerable<Role>> GetUserRolesAsync(User user)
        {
            var foundUser = await DbSet.Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return foundUser?.Roles ?? Enumerable.Empty<Role>();
        }

        public async Task<User?> GetDetailedInfoByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            return await DbSet
                .Include(u => u.RefreshTokens)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
        {
            return await DbSet.Include(u => u.Roles).ToArrayAsync();
        }

        public async Task<bool> Exists(string phoneNumber)
        {
            return await DbSet.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
