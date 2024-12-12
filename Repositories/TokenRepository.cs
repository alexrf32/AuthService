using AuthService.Data;
using AuthService.Models;
using AuthService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly DataContext _context;

        public TokenRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddToBlacklist(string token)
        {
            if (await IsTokenRevoked(token)) return;

            _context.RevokedTokens.Add(new RevokedToken
            {
                Token = token,
                RevokedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenRevoked(string token)
        {
            return await _context.RevokedTokens.AnyAsync(rt => rt.Token == token);
        }

        // Implementación de RemoveFromBlacklist
        public async Task RemoveFromBlacklist(string token)
        {
            var revokedToken = await _context.RevokedTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (revokedToken != null)
            {
                _context.RevokedTokens.Remove(revokedToken);
                await _context.SaveChangesAsync();
            }
        }
    }
}
