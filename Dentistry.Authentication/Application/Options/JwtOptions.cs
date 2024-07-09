namespace Application.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double TokenLifeTimeInMinutes { get; set; }
    public double RefreshTokenLifeTimeInMinutes { get; set; }
}