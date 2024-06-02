namespace Application.DTO
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset ExpiredDateTime { get; set; }
        public Guid UserId { get; set; }
    }
}