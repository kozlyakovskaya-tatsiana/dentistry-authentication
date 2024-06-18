using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        private readonly IPasswordHasher _passwordHasher;

        public UsersRepository(DentistryAuthenticationContext dentistryAuthenticationContext, IPasswordHasher passwordHasher)
            : base(dentistryAuthenticationContext)
        {
            _passwordHasher = passwordHasher;
        }

        // Consider moving this to a service if it is not directly related to data access.
        public bool CheckPassword(User user, string password)
        {
            if (user == null) 
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(password)) 
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            return _passwordHasher.Verify(password, user.PasswordHash);
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(User user)
        {
            if (user == null) 
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            var foundUser = await DbSet.Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return foundUser?.Roles ?? Enumerable.Empty<Role>();
        }

        public async Task<User?> FindByPhoneNumberAsync(string phoneNumber, bool includeRefreshTokens = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));

            var query = DbSet.AsQueryable();

            if (includeRefreshTokens)
            {
                query = query.Include(u => u.RefreshTokens);
            }

            return await query.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
        {
            return await DbSet.Include(u => u.Roles).ToArrayAsync();
        }

    }
}
