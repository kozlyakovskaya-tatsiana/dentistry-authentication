using Application.Exceptions;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Features.UsersManagement.Commands
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
        private readonly IPasswordHashService _passwordHashService;

        public CreateUserCommandHandler(IUsersRepository usersRepository, IPasswordHashService passwordHashService, IRolesRepository rolesRepository)
        {
            _usersRepository = usersRepository;
            _passwordHashService = passwordHashService;
            _rolesRepository = rolesRepository;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var role = await _rolesRepository.GetAsync(request.RoleId);
            if (role == null)
            {
                throw new ArgumentException(nameof(request.RoleId));
            }

            var isUserAlreadyExisting = await _usersRepository.Exists(request.PhoneNumber);

            if (isUserAlreadyExisting)
                throw new EntityAlreadyExistException($"User with phone number {request.PhoneNumber} already exists.");   

            var passwordHash = _passwordHashService.Hash(request.Password);

            var user = User.Create(request.PhoneNumber, request.Email, passwordHash, new[] { role }, null);

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
