using AuthService.Data;
using AuthService.Repositories;
using AuthService.Repositories.Interfaces;
using AuthService.Services;
using AuthService.Services.Interfaces;
using AuthService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuración del contexto de la base de datos
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing"))
            )
        };
    });

// Registro de dependencias (DI)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<AuthenticationService>();

// Configuración de RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["RabbitMQ:HostName"], // Deberías configurar RabbitMQ en appsettings.json
        UserName = builder.Configuration["RabbitMQ:UserName"],
        Password = builder.Configuration["RabbitMQ:Password"]
    };
    return factory.CreateConnection();
});

builder.Services.AddSingleton<IModel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    return connection.CreateModel();
});

// Configuración adicional
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
