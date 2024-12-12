namespace AuthService.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string email, string role);
        bool ValidateToken(string token); 
        string GetEmailFromToken(string token); 
    }
}
