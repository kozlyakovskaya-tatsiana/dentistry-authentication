using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class UserRoleRepository : BaseRepository<Role>, IUserRolesRepository
    {
        public UserRoleRepository(DentistryAuthenticationContext dentistryAuthenticationContext) : base(dentistryAuthenticationContext)
        {
        }
    }
}
