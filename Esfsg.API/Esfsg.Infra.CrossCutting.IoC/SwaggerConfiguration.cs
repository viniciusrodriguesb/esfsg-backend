using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Esfsg.Infra.CrossCutting.IoC
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Swagger Documentação Web API",
                    Contact = new OpenApiContact() { Name = "ESFRSG", Email = "suporte@esfrsg.com" }
                });
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Please insert token",
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.Http,
                //    BearerFormat = "JWT",
                //    Scheme = "bearer"
                //});
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
            });

            return services;
        }
    }
}
