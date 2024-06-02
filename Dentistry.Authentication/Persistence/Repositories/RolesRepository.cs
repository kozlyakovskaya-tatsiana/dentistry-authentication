using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class RolesRepository : BaseRepository<Role>, IRolesRepository
    {
        public RolesRepository(DentistryAuthenticationContext dentistryAuthenticationContext) : base(dentistryAuthenticationContext)
        { }
    }
}
