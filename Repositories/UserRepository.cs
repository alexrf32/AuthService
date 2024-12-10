using AuthService.Data;
using AuthService.Models;
using AuthService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
    }
}
