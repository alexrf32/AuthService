namespace AuthService.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(string email, string role);
}
