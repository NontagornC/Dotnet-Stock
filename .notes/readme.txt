Vscode extension
- C#
- C...
- Essential ASP.net

Shortcut keyboard
--- 
cmd + . : to show quick actions
initial field from  


- Data Annoation
- Route 
 + [Route("aaaa")]  // ....localhost:5001/aaaa
 + [Route("weatherforecast")]  // ....localhost:5001/weatherforecast
 + [Route("[controller]")] /// ... mapping to controller class *Controller
- Set port : launchSettings.json    
 + HttpGet[]

Startup.cs => Program.cs
# all services are added in Program.cs  
# ex: builder.Services.AddSwaggerGen();

Swagger
app.UseSwaggerUI(c=> c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_hero"));
 https://localhost:7000/swagger/v1/swagger.json
 https://localhost:7000/swagger/index.html


Status
Ok()
NotFound()
NoContection
CreatedAt(...)
Authorization()
StatusCode(401)


docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Tel1234!' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU14-ubuntu-20.04

# Setup sql server on M1
# https://medium.com/geekculture/docker-express-running-a-local-sql-server-on-your-m1-mac-8bbc22c49dc9
docker run -e "ACCEPT_EULA=1" -e 'MSSQL_SA_PASSWORD=Tel1234!' -e "MSSQL_PID=Developer" -e "MSSQL_USER=SA" -p 1433:1433 -d --name=sql mcr.microsoft.com/azure-sql-edge

dotnet add package Microsoft.EntityFrameworkCore --version 6.0.1

https://github.com/dotnet/cli-lab/releases
https://docs.microsoft.com/en-us/dotnet/core/additional-tools/uninstall-tool?tabs=macos

# uninstall
sudo ./dotnet-core-uninstall remove --sdk  5.0.400
sudo ./dotnet-core-uninstall remove --runtime  5.0.9
./dotnet-core-uninstall list


# Remove onConfigure
to hide password


https://github.com/MapsterMapper/Mapster
Mapster


public => internal 


# disable browser lanucher when run or build 
https://elanderson.net/2020/09/dont-launch-a-browser-running-asp-net-core-back-end-created-from-web-template-studio/

# Remove
"serverReadyAction": {
                "action": "openExternally",
                "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
            },


EF6 - Annotation to allow empty string but not allow null
https://stackoverflow.com/questions/38770300/ef6-annotation-to-allow-empty-string-but-not-allow-null
[Required(AllowEmptyStrings = true)]

#DTO
[Required]
[MaxLength]
[Range]


# register service
builder.Services.AddTransient<IProductService, ProductService>();

# auto register services
AutoFac
AutoFacDependencyInjection

# Publish static file
app.UseStaticFiles();

# service installer
- Create Installers folder
- Create interface IInstallers
- Create C# extension InstallerExtensions
public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration){
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x => 
            typeof(IInstallers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance).Cast<IInstallers>().ToList();
            installers.ForEach(installer => installer.InstallServices(services, configuration));            
        }
- Map individual services to installer (implement IInstaller)
- In Program.cs, replacing all services setting with the extension
// Add services to the container.
builder.Services.InstallServiceInAssembly(builder.Configuration);


# Cors
- add CorsInstaller.cs
    services.AddCors(options =>
            {
                // Specifi policy
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins(
                        "https://www.w3schools.com",
                        "http://www.localhost:7000"
                    ).AllowAnyHeader().AllowAnyMethod();
                });

                // Allow all 
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
------------------------
- app.UseCors("AllowSpecificOrigins");
