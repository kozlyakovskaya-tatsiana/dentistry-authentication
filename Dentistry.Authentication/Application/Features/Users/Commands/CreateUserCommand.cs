using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IRolesRepository rolesRepository)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _rolesRepository = rolesRepository;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var role = await _rolesRepository.GetAsync(request.RoleId);
            if (role == null)
            {
                throw new ArgumentException(nameof(request.RoleId));
            }
            var user = new User(request.PhoneNumber, request.Email, request.Password, new[] { role }, _passwordHasher);

            _usersRepository.Create(user);

            await _usersRepository.SaveAsync();
        }
    }

    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
