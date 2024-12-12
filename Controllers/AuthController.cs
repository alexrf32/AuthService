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

    // Ruta para iniciar sesión
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authenticationService.LoginAsync(loginRequestDto);
        return Ok(response);
    }

    // Ruta para registrar un nuevo usuario
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterStudentDto registerStudentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _authenticationService.RegisterAsync(registerStudentDto);
        return CreatedAtAction(nameof(Login), new { email = registerStudentDto.Email }, registerStudentDto);
    }

    // Ruta para actualizar la contraseña
    [HttpPut("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _authenticationService.UpdatePasswordAsync(updatePasswordDto);
        return NoContent();
    }

    // Ruta para revocar el token
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeToken([FromBody] string token)
    {
        await _authenticationService.RevokeTokenAsync(token);
        return NoContent();
    }

    // Ruta para validar el token
    [HttpGet("validate")]
    public async Task<ActionResult<bool>> ValidateToken([FromQuery] string token)
    {
        var isValid = await _authenticationService.ValidateTokenAsync(token);
        return Ok(isValid);
    }
}
