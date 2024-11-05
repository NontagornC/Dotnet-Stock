namespace dotnet_stock.Installers
{
    public interface IInstallers
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}