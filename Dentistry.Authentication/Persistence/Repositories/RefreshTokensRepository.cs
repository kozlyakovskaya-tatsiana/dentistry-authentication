using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class RefreshTokensRepository : BaseRepository<RefreshToken>, IRefreshTokensRepository
    {
        public RefreshTokensRepository(DentistryAuthenticationContext context) : base(context)
        {
        }
    }
}
