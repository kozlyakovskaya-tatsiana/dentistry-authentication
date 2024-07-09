using Domain.Entities;
using Domain.IRepositories;

namespace Persistence.Repositories;

public class RolesRepository : BaseRepository<Role>, IRolesRepository
{
    public RolesRepository(DentistryAuthenticationContext context) : base(context)
    { }
}