using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRolesRepository
    {
        public UserRoleRepository(DentistryAuthenticationContext dentistryAuthenticationContext) : base(dentistryAuthenticationContext)
        {
        }
    }
}
