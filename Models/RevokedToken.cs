namespace AuthService.Models;

public class RevokedToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime RevokedAt { get; set; }
}
