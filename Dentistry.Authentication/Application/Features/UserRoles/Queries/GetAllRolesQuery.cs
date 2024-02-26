using Application.DTO.UserRoles;
using Domain;
using Domain.Enumerations;
using Domain.Repositories;
using Mapster;
using MediatR;

namespace Application.Features.UserRoles.Queries
{
    public class GetAllRolesQuery : IRequest<IEnumerable<UserRoleDto>>
    {
        public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<UserRoleDto>>
        {
            private readonly IUserRolesRepository _userRolesRepository;
            public GetAllRolesQueryHandler(IUserRolesRepository userRolesRepository)
            {
                _userRolesRepository = userRolesRepository;
            }
            public async Task<IEnumerable<UserRoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
            {
                var roles = await _userRolesRepository.GetAllAsync();

                var roles2 = Domain.Enumeration.GetAll<UserRoleType>();
                return roles.AsQueryable().ProjectToType<UserRoleDto>().ToArray();
            }
        }
    }
}


// VIEWS and DTO


// UserRoleViewModel