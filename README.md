AuthService - Gestión de Autenticación
AuthService es un microservicio encargado de manejar la autenticación en un sistema distribuido. Sus principales responsabilidades incluyen:

Emisión de tokens JWT para usuarios autenticados.
Validación de tokens.
Revocación de tokens mediante una lista negra (blacklist).
Requisitos previos
Antes de ejecutar este proyecto, asegúrate de tener instalados los siguientes componentes:

.NET 8.0 SDK
SQL Server
Entity Framework Core Tools

Configuración inicial
1. Clona el repositorio
bash
Copiar código
git clone https://github.com/alexrf32/AuthService.git
cd AuthService
2. Configura el archivo appsettings.json
Modifica las configuraciones en el archivo appsettings.json según tu entorno:

json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "YourSuperSecretKey12345",
    "Issuer": "AuthService",
    "Audience": "AuthServiceUsers"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=AuthServiceDB;Trusted_Connection=True;"
  }
}
3. Restaurar dependencias
Ejecuta el siguiente comando para restaurar las dependencias del proyecto:
dotnet restore

4. Migrar la base de datos
Ejecuta las migraciones para configurar la base de datos:
dotnet ef database update

Ejecución del proyecto
Para iniciar el microservicio, usa el siguiente comando:
dotnet run
Esto levantará el servicio en http://localhost:5095.

Endpoints
1. Login
URL: /api/auth/login
Método: POST
Descripción: Genera un token JWT para un usuario autenticado.
2. Revocar token
URL: /api/auth/revoke
Método: POST
Descripción: Revoca un token específico.
3. Validar token
URL: /api/auth/validate
Método: GET
Descripción: Verifica si un token es válido.
Query Parameter: ?token=<TOKEN>

Pruebas
Swagger
Cuando el servicio esté en ejecución, accede a http://localhost:5095/swagger para explorar y probar los endpoints mediante Swagger UI.

Postman
Puedes importar los siguientes endpoints en Postman para realizar pruebas:

Login
Revoke
Validate