using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(DentistryAuthenticationContext dentistryAuthenticationContext) : base(dentistryAuthenticationContext)
        {
        }
    }
}
