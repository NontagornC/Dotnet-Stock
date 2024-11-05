using dotnet_stock.Data;
using dotnet_stock.Installers;
using Microsoft.EntityFrameworkCore;

namespace dotnet_hero.Installers
{
    public class DatabaseInstaller : IInstallers
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                  options.UseSqlServer(configuration.GetConnectionString("ConnectionSQLServer"))
            );
        }
    }
}
