namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string PasswordHash { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
