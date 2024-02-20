namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public DateTimeOffset ExpiredDateTime { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
