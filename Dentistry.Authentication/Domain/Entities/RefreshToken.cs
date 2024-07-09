namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        private RefreshToken() {}
        public string Token { get; private init; }
        public DateTimeOffset ExpireDateTime { get; private init; }
        public User User { get; private init; }
        public Guid UserId { get; private init; }

        public static RefreshToken Create(string token, DateTimeOffset expireDateTime, User user)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            return new RefreshToken()
            {
                Token = token, 
                ExpireDateTime = expireDateTime,
                User = user,
                UserId = user.Id
            };
        }
    }
}
