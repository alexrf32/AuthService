using AuthService.Data; 
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories;

public class TokenRepository
{
    private readonly DataContext _context;

    public TokenRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddToBlacklist(string token)
    {
        _context.RevokedTokens.Add(new RevokedToken { Token = token, RevokedAt = DateTime.UtcNow });
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsTokenRevoked(string token)
    {
        return await _context.RevokedTokens.AnyAsync(rt => rt.Token == token);
    }
}
