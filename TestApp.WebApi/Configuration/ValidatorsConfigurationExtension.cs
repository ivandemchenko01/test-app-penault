using FluentValidation.AspNetCore;

namespace TestApp.WebApi.Configuration
{
    public static class ValidationConfigurationExtensions
    {
        public static IServiceCollection ConfigureValidation(this IServiceCollection services)
        {
            services.AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<Program>());

            return services;
        }
    }
}
