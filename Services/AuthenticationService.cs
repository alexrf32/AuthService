using AuthService.DTOs;
using AuthService.Repositories;
using AuthService.Services;
using System;
using System.Threading.Tasks;
using BCrypt.Net;
using AuthService.Repositories.Interfaces;
using AuthService.Services.Interfaces;

namespace AuthService.Services
{
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ITokenRepository _tokenRepository;

        public AuthenticationService(IUserRepository userRepository, IJwtService jwtService, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _tokenRepository = tokenRepository;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email)
                ?? throw new UnauthorizedAccessException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.HashedPassword))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = _jwtService.GenerateToken(user.Email, user.Role.Name);

            return new LoginResponseDto
            {
                Email = user.Email,
                Role = user.Role.Name,
                Token = token
            };
        }

        public async Task RevokeTokenAsync(string token)
        {
            await _tokenRepository.AddToBlacklist(token);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return !await _tokenRepository.IsTokenRevoked(token);
        }
    }
}
