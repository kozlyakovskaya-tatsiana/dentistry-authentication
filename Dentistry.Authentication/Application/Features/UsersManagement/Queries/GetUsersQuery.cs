using Application.DTO;
using Domain.Repositories;
using Mapster;
using MediatR;

namespace Application.Features.UsersManagement.Queries
{
    public record GetUsersQuery : IRequest<IEnumerable<UserDto>> { }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUsersRepository _usersRepository;

        public GetUsersQueryHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _usersRepository.GetUsersWithRolesAsync();

            return users.Adapt<IEnumerable<UserDto>>();
        }
    }
}
