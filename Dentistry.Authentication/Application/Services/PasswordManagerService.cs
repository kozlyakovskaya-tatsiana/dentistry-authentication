using Application.Services.Interfaces;

namespace Application.Services
{
    public class PasswordManagerService : IPasswordManagerService
    {
        private readonly IPasswordHashService _passwordHashService;

        public PasswordManagerService(IPasswordHashService passwordHashService)
        {
            // Should be custom exception to return 500 to ui?
            _passwordHashService = passwordHashService ?? throw new NullReferenceException(nameof(passwordHashService));
        }

        public bool CheckPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password and password hash must have not null and not empty value.");
            }

            return _passwordHashService.Verify(password, passwordHash);
        }
    }
}
