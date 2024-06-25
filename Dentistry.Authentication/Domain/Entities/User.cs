namespace Domain.Entities
{
    public class User : BaseEntity
    {
        private User(){ }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set;  }
        public IEnumerable<RefreshToken>? RefreshTokens { get; private set; }
        public IEnumerable<Role> Roles { get; private set; }
        public void SetRefreshTokens(IEnumerable<RefreshToken>? refreshTokens)
        {
            RefreshTokens = refreshTokens ?? Enumerable.Empty<RefreshToken>() ;
        }
        public static User Create(
            string phoneNumber,
            string email,
            string passwordHash,
            IEnumerable<Role>? roles,
            IEnumerable<RefreshToken>? refreshTokens)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentNullException(nameof(phoneNumber), "Phone number cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentNullException(nameof(passwordHash), "Password hash cannot be null or empty.");
            }

            var rolesArray = roles?.ToArray() ?? new Role[] { };

            if (!rolesArray.Any())
            {
                throw new ArgumentException("Roles cannot be null or empty.", nameof(roles));
            }

            return new User
            {
                PhoneNumber = phoneNumber,
                Email = email,
                PasswordHash = passwordHash,
                Roles = rolesArray,
                RefreshTokens = refreshTokens,
            };
        }
    }
}
