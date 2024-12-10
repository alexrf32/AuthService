namespace AuthService.Repositories.Interfaces;

public interface ITokenRepository
{
    Task AddToBlacklist(string token);
    Task<bool> IsTokenRevoked(string token);
}
