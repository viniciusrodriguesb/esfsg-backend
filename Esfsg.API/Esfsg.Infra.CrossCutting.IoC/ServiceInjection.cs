using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Esfsg.Infra.CrossCutting.IoC
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            DatabaseConfiguration.ConfigureDatabase(services, configuration);
            ConfigureServices(services);
            CORSConfiguration.ConfigureCORS(services);

            return services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {

        }

    }
}
