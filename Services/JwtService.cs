using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services;

public class JwtService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtService(IConfiguration configuration)
    {
        _key = configuration["Jwt:Key"];
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
    }

    public string GenerateToken(string email, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
