using AuthService.Models;

namespace AuthService.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name);
}
