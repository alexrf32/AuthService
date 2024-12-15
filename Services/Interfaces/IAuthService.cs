using AuthService.DTOs;

namespace AuthService.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    Task<LoginResponseDto> RegisterAsync(RegisterStudentDto registerStudentDto);
    Task UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto);
}
