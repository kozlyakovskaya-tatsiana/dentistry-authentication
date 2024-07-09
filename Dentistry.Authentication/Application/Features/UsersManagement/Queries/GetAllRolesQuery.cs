using Application.DTO;
using Domain.IRepositories;
using Mapster;
using MediatR;

namespace Application.Features.UsersManagement.Queries;

public record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>> { }

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRolesRepository _rolesRepository;
    public GetAllRolesQueryHandler(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }
    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _rolesRepository.GetAllAsync();

        return roles.Adapt<IEnumerable<RoleDto>>();
    }
}