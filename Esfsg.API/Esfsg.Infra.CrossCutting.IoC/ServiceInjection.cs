using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Esfsg.Infra.CrossCutting.IoC
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddHangfireServer();

            DatabaseConfiguration.ConfigureDatabase(services, configuration);
            DatabaseConfiguration.ConfigureDatabaseHangfire(services, configuration);
            CORSConfiguration.ConfigureCORS(services);
            ServicesConfiguration.ConfigureServices(services);

            return services;
        }
    }
}
