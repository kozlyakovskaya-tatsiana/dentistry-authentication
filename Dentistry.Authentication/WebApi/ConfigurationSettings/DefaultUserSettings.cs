namespace WebApi.ConfigurationSettings
{
    public class DefaultUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
    public class DefaultUserSettings
    {
        public IEnumerable<DefaultUser> DefaultUsers { get; set; }
    }
}
