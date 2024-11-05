namespace dotnet_stock.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            // งง
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
            typeof(IInstallers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance).Cast<IInstallers>().ToList();
            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}