using MediatR;

namespace Application.Features.Authentication.Commands
{
    public class LoginUserCommand : IRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
