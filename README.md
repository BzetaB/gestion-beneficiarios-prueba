# Gestión de Beneficiarios - Backend
Backend desarrollado en **ASP.NET Core Web API (.NET 10)** para la gestión de beneficiarios y documentos de identidad.
Utiliza **SQL Server** desplegado en **Docker** y sigue buenas prácticas de validación y arquitectura.

---
## Tecnologías
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- SQL Server (Docker)
- Git para control de versiones
---

## Configuración de SQL Server con Docker

1. Crear un contenedor de SQL Server:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name sqlserver-container -d mcr.microsoft.com/mssql/server:2022-latest
```

2. Ejecutar los Scripts que se encuentran en la carpeta ScriptsDBSqlServer 
- Lo puedes hacer usando SQL Server Management Studio (SSMS) conectándote a:
```
 Servidor: localhost,1433
 Usuario: sa
 Contraseña: YourStrong!Passw0rd
 ```
Recuerda marcar el check en Certificado de servidor de confianza en SSMS al conectarte.


---

## Configuración del proyecto
1. Clona el repositorio:

```cmd
git clone <repo-url>
cd gestion-beneficiarios
```
2. Configura las variables de entorno necesarias usando dotnet user-secrets:

```cmd
dotnet user-secrets set "DB_SERVER" "localhost,1433"
dotnet user-secrets set "DB_NAME" "GestionBeneficiariosDB"
dotnet user-secrets set "DB_USER" "sa"
dotnet user-secrets set "DB_PASSWORD" "YourStrong!Passw0rd"
```

3. Ejecuta el proyecto
