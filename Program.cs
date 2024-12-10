using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.Data;
using AuthService.Repositories;
using AuthService.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de JWT
var jwtKey = builder.Configuration["Jwt:Key"] 
             ?? throw new InvalidOperationException("Jwt:Key is required in the configuration.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] 
                ?? throw new InvalidOperationException("Jwt:Issuer is required in the configuration.");
var jwtAudience = builder.Configuration["Jwt:Audience"] 
                  ?? throw new InvalidOperationException("Jwt:Audience is required in the configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Configuración de la base de datos
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de servicios e inyección de dependencias
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<TokenRepository>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
