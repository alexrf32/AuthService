using AuthService.Data;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories;

public class RoleRepository
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

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles.FindAsync(id);
    }
}
