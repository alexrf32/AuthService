using AuthService.DTOs;
using AuthService.Repositories;
using AuthService.Services.Interfaces;
using AuthService.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using BCrypt.Net;
using AuthService.Models;

namespace AuthService.Services
{
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ITokenRepository _tokenRepository;

        public AuthenticationService(IUserRepository userRepository, IJwtService jwtService, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        }

        // Método de Login
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null)
                throw new ArgumentNullException(nameof(loginRequestDto), "Login request cannot be null");

            // Buscar el usuario por su correo electrónico
            var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email);
            
            // Si el usuario no existe, lanzamos una excepción
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // Verificar la contraseña proporcionada con la contraseña hasheada
            if (!BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.HashedPassword))
                throw new UnauthorizedAccessException("Invalid email or password.");

            // Generar el token JWT
            var token = _jwtService.GenerateToken(user.Email, user.Role.Name);

            // Devolver la respuesta con el token, el correo y el rol
            return new LoginResponseDto
            {
                Email = user.Email,
                Role = user.Role.Name,
                Token = token
            };
        }

        // Método para Revocar Token
        public async Task RevokeTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token cannot be null or empty", nameof(token));

            // Agregar el token a la lista de revocados
            await _tokenRepository.AddToBlacklist(token);
        }

        // Método para Validar Token
        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token cannot be null or empty", nameof(token));

            // Verificar si el token está revocado
            return !await _tokenRepository.IsTokenRevoked(token);
        }

        // Método para Registrar un Nuevo Usuario
        public async Task RegisterAsync(RegisterStudentDto registerStudentDto)
        {
            if (registerStudentDto == null)
                throw new ArgumentNullException(nameof(registerStudentDto), "Register request cannot be null");

            // Verificar si el correo electrónico ya está registrado
            var existingUser = await _userRepository.GetByEmailAsync(registerStudentDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already exists.");

            // Hashear la contraseña proporcionada
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerStudentDto.Password);

            // Crear el nuevo usuario
            var user = new User
            {
                Name = registerStudentDto.Name,
                FirstLastName = registerStudentDto.FirstLastName,
                SecondLastName = registerStudentDto.SecondLastName,
                RUT = registerStudentDto.RUT,
                Email = registerStudentDto.Email,
                HashedPassword = hashedPassword,
                CareerId = registerStudentDto.CareerId,
                RoleId = 2 // Asignar un rol por defecto (puedes ajustarlo según tus necesidades)
            };

            // Guardar el usuario en la base de datos
            await _userRepository.AddAsync(user);
        }

        // Método para Actualizar la Contraseña de un Usuario
        public async Task UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto)
        {
            if (updatePasswordDto == null)
                throw new ArgumentNullException(nameof(updatePasswordDto), "Update password request cannot be null");

            // Buscar el usuario por su correo
            var user = await _userRepository.GetByEmailAsync(updatePasswordDto.Email)
                ?? throw new UnauthorizedAccessException("User not found");

            // Verificar si la contraseña actual es correcta
            if (!BCrypt.Net.BCrypt.Verify(updatePasswordDto.CurrentPassword, user.HashedPassword))
                throw new UnauthorizedAccessException("Current password is incorrect");

            // Hashear la nueva contraseña
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.Password);

            // Actualizar la contraseña en la base de datos
            user.HashedPassword = hashedPassword;
            await _userRepository.UpdateAsync(user);
        }
    }
}
