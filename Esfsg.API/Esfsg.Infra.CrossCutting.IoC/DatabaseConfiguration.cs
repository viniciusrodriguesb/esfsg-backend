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

            services.AddDbContext<DbContextBase>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null
                    );
                    npgsqlOptions.CommandTimeout(30);
                });
            });
        }

        public static void ConfigureDatabaseHangfire(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HangfireConfiguration>(configuration.GetSection("HangfireConfiguration"));
            var connectionString = configuration.GetConnectionString("databaseConnection");

            services.AddHangfire(options =>
            {
                options.UseConsole()
                        .UsePostgreSqlStorage(o =>
                        {
                            o.UseNpgsqlConnection(connectionString);
                        },
                        new PostgreSqlStorageOptions
                        {
                            PrepareSchemaIfNecessary = true,
                            QueuePollInterval = TimeSpan.FromSeconds(30),
                            InvisibilityTimeout = TimeSpan.FromMinutes(30),
                            DistributedLockTimeout = TimeSpan.FromMinutes(10),
                            JobExpirationCheckInterval = TimeSpan.FromHours(1)
                        });
            });
        }

    }
}
