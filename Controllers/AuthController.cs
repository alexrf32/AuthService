using Microsoft.AspNetCore.Mvc;
using AuthService.DTOs;
using AuthService.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;

    public AuthController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var response = await _authenticationService.LoginAsync(loginRequestDto);
        return Ok(response);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeToken([FromBody] string token)
    {
        await _authenticationService.RevokeTokenAsync(token);
        return NoContent();
    }

    [HttpGet("validate")]
    public async Task<ActionResult<bool>> ValidateToken([FromQuery] string token)
    {
        var isValid = await _authenticationService.ValidateTokenAsync(token);
        return Ok(isValid);
    }
}
