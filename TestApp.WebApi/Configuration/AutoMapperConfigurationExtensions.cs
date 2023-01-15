using System.Reflection;

namespace TestApp.WebApi.Configuration
{
    public static class AutoMapperConfigurationExtensions
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(options =>
            {
                options.AddMaps(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
