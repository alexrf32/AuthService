using AuthService.Data;
using AuthService.Models;
using AuthService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DataContext _context;

    public RoleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
}
