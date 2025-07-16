using Esfsg.Application.DTOs;
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

            services.AddDbContext<DbContextBase>(options => options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
                npgsqlOptions.CommandTimeout(60);
            }));
        }

        public static void ConfigureDatabaseHangfire(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HangfireConfiguration>(configuration.GetSection("HangfireConfiguration"));
            var connectionString = configuration.GetConnectionString("databaseConnection");

            services.AddHangfire(options =>
            {
                options.UseConsole().UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(5),
                    InvisibilityTimeout = TimeSpan.FromMinutes(5),
                    DistributedLockTimeout = TimeSpan.FromMinutes(5)
                });
            });
        }

    }
}
