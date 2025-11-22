using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Esfsg.Infra.CrossCutting.IoC
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddMemoryCache();
            QuestPDF.Settings.License = LicenseType.Community;

            DatabaseConfiguration.ConfigureDatabase(services, configuration);
            CORSConfiguration.ConfigureCORS(services);
            ServicesConfiguration.ConfigureServices(services);

            return services;
        }

        public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            DatabaseConfiguration.ConfigureDatabaseHangfire(services, configuration);
            services.AddHangfireServer();

            AddServices(services, configuration);

            return services;
        }

    }
}
