using Esfsg.Infra.Data;
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
    }
}
