# Blog blazor Web Assembly #
### Sistema desarrollado en Net 8, utilizando Microsoft SQL Server para persistencia de datos. ###
___
### Permite registrar usuarios, así como autenticarse y permitir un control de los post que genere; una vez que crea un post, puede consultar y visualizarlos en la ruta "/" equivalente a home, así como poder navegar a una tabla de posts en la ruta "/posts", permitiendo modificar o eliminar dicho post. ### 
___
## Es necesario tener instaladas los siguientes paquetes NuGet ##
- automapper.extensions.microsoft.dependencyinjection\8.1.1\
- microsoft.entityframeworkcore.sqlserver\8.0.20\
- swashbuckle.aspnetcore\6.6.2\
- newtonsoft.json\13.0.4\
- microsoft.aspnetcore.mvc.newtonsoftjson\8.0.20\
- xact.core.pcl\0.0.5014\
- microsoft.aspnetcore.openapi\8.0.20\
- microsoft.aspnetcore.authentication.jwtbearer\8.0.20\
- microsoft.entityframeworkcore.tools\8.0.20\

## Configuración del appsettings.json ##
```json
{
  "ApiSettings": {
    "Secreta": "0f8fad5b-d9cb-469f-a165-70867728950e"
  },
  "ConnectionStrings": {
    "ConexionSql": "Server=(localdb)\\mssqllocaldb;Database=BlogBDBlazorWASM;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```