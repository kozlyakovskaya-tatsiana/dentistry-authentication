using Application.DTO;
using Domain.IRepositories;
using Mapster;
using MediatR;

namespace Application.Features.UsersManagement.Queries;

public record GetUsersQuery : IRequest<IEnumerable<UserWithRolesDto>> { }

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserWithRolesDto>>
{
    private readonly IUsersRepository _usersRepository;

    public GetUsersQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<IEnumerable<UserWithRolesDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _usersRepository.GetUsersWithRolesAsync();

        return users.Adapt<IEnumerable<UserWithRolesDto>>();
    }
}