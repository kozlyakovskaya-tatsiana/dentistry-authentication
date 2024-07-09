using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UsersRepository : BaseRepository<User>, IUsersRepository
{
    public UsersRepository(DentistryAuthenticationContext context)
        : base(context)
    { }
    public async Task<User?> GetUserAsync(string phoneNumber)
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