# AuthService: Microservicio de Gestión de Autenticación

AuthService es un microservicio desarrollado en .NET 8 que maneja la autenticación en un sistema distribuido. Sus principales funciones incluyen:

- Emisión de tokens JWT para usuarios autenticados.
- Validación de tokens JWT.
- Revocación de tokens mediante una lista negra (blacklist).

---

## Características Principales

- **Autenticación Segura**: Uso de JWT para gestionar la autenticación.
- **Integración con RabbitMQ**: Publicación de eventos relacionados con la autenticación.
- **Base de Datos SQL Server**: Almacenamiento de usuarios, roles y tokens revocados.
- **Endpoints RESTful**: Diseño accesible y estandarizado para integrarse con otras aplicaciones.

---

## Requerimientos

Asegúrate de que tu entorno cumple con los siguientes requisitos:

- **[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)** 
- **[SQL Server](https://www.microsoft.com/sql-server/)** 
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)** 
- **[RabbitMQ](https://www.rabbitmq.com/)** para la mensajería.
- **[Postman](https://www.postman.com/downloads/)** o **Swagger** para probar los endpoints.

---

## Configuración Inicial

### Clonar el Repositorio

Clona el repositorio con el siguiente comando:

```bash
git clone https://github.com/alexrf32/AuthService.git
cd AuthService
