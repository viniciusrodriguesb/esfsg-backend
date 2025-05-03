using Esfsg.Infra.Data;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Esfsg.Infra.CrossCutting.IoC
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("databaseConnection");

            services.AddDbContext<DbContextBase>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );
        }

        public static void ConfigureDatabaseHangfire(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("databaseTesteConnection");

            services.AddHangfire(options =>
            {
                options.UseConsole()
                       .UsePostgreSqlStorage(connectionString);
            });
        }

    }
}
