using Microsoft.Extensions.DependencyInjection;
using TestApp.Core.Interfaces;
using TestApp.Core.Providers;

namespace TestApp.Core.Configuration;

public static class CryptoProviderConfiguration
{
    public static IServiceCollection ConfigureCryptoProviders(this IServiceCollection services)
    {
        services.AddSingleton<ICryptoProvider, BinanceCryptoProvider>();
        services.AddSingleton<ICryptoProvider, KucoinCryptoProvider>();
        
        return services;
    }
}

