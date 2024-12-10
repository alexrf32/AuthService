namespace AuthService.DTOs;

public class LoginResponseDto
{
    public string Name { get; set; } = null!;
    public string FirstLastName { get; set; } = null!;
    public string SecondLastName { get; set; } = null!;
    public string RUT { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Career { get; set; } = null!;
    public string Token { get; set; } = null!;
}
