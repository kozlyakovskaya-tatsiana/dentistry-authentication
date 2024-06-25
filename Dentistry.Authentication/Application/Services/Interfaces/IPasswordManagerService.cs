namespace Application.Services.Interfaces
{
    public interface IPasswordManagerService
    {
        bool CheckPassword (string password, string passwordHash);
    }
}
