using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestApp.Core.Interfaces;

namespace TestApp.Core.Configuration;

public static class CryptoProviderConfiguration
{
    public static IServiceCollection ConfigureCryptoProviders(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes();
        var providers = types.Where(type => type.IsClass && typeof(ICryptoProvider).IsAssignableFrom(type)).ToList();
        
        
        foreach (var provider in providers)
            services.AddSingleton(typeof(ICryptoProvider), provider);

        //var test3 = types.Where(type => type.IsClass && typeof(ICryptoProvider).IsAss.(type)).ToList();
        return services;
    }
}

