# Shop Online API
 API RESTful que gestiona un modelo de negocio de una tienda en línea
 ## Instalación
* Instalar la SDK 6 de [Net Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* Descargar el proyecto del repositorio (https://github.com/JoannyCuba/ShopOnlineAPI.git) 
* Configurar el acceso a la base de datos en el archivo appsettings.json y appsettings.Development.json en modo desarrollo,según la cadena de conexión del proveedor a configurar
	```
	"Database": {
                "Provider": "mssql",
                "ConnectionString": "server=localhost;user=sa;password=password;database=ShopOnline"
                },
	```
El proveedor de base de datos soportado es SqlServer("mssql").
* Ejecutar en la raiz del proyecto
	```
	dotnet run --project ShopOnlineApi
	```
Automaticamente se ejecutan las migraciones del proyecto en la base de datos seleccionada, sin ninguna intervención del desarrollador.
En las migraciones se crea un cliente admin que permite la obtención del token de acceso a los métodos de los controladores con Id:  `8a946576-f9df-4b51-9187-594dacc2f9cd`, Name: Super, Email: superadmin@gmail.com y Password: Passw0rd
* Con estos datos se podrá obtener un token de acceso.

## Ruta de la API
```
http://localhost:5200/swagger
https://localhost:5201/swagger

```
## Ruta de la Documentación OpenApi
```
http://localhost:5200/swagger/v1/swagger.json
https://localhost:5201/swagger/v1/swagger.json
```
