namespace AuthService.DTOs;
public class TokenValidationResponseDto
{
    public bool IsValid { get; set; }

    public string? Reason { get; set; }
}
