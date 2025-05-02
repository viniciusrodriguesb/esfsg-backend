using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Esfsg.Infra.CrossCutting.IoC
{
    public static class FluentValidationConfiguration
    {

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            //services.AddValidatorsFromAssemblyContaining(typeof());

            return services;
        }
    }
}
