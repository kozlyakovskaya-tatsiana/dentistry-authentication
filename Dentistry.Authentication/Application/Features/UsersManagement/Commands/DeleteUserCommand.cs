using Application.Exceptions;
using Domain.IRepositories;
using FluentValidation;
using MediatR;

namespace Application.Features.UsersManagement.Commands;

public class DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }
}
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUsersRepository _usersRepository;

    public DeleteUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository ?? throw new ServiceNotLoadedException(nameof(usersRepository));
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _usersRepository.DeleteAsync(request.Id);
    }
}
public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("User id is required for delete operation.");
    }
}