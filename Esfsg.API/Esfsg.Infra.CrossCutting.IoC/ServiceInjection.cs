using Esfsg.Application.Interfaces;
using Esfsg.Application.Services;
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

            DatabaseConfiguration.ConfigureDatabase(services, configuration);
            DatabaseConfiguration.ConfigureDatabaseHangfire(services, configuration);
            CORSConfiguration.ConfigureCORS(services);
            JobsInjection.AddJobs(services);
            ConfigureServices(services);

            return services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();


            services.AddScoped<IClasseService, ClassesService>();
            services.AddScoped<IFuncoesService, FuncoesService>();
            services.AddScoped<IInstrumentoService, InstrumentoService>();
            services.AddScoped<ICondicaoMedicaService, CondicaoMedicaService>();
            services.AddScoped<IEventoService, EventoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IInscricaoService, InscricaoService>();
            services.AddScoped<IPixService, PixService>();
        }

    }
}
