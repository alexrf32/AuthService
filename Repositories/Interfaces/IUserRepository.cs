using AuthService.Models;

namespace AuthService.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}
