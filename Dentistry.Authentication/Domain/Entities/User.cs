using Domain.Services;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public User(string phoneNumber, string email, string password, IEnumerable<Role> roles, IPasswordHasher passwordHasher)
        {
            PhoneNumber = phoneNumber;
            Email = email;
            PasswordHash = passwordHasher.Hash(password);
            Roles = roles.ToList();
        }
        private User() { } 
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        public List<Role> Roles { get; set; }
    }
}
