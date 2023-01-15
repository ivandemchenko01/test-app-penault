
using TestApp.Core.Interfaces;
using TestApp.Core.Providers;
using TestApp.Domain.Models;

namespace TestApp.Core;
public class ExchangeRatesUpdater
{
    private readonly IEnumerable<ICryptoProvider> _providers;
    public event Action<ECryptoProvider,List<ExchangePair>> OnRatesUpdate;


    public ExchangeRatesUpdater(IEnumerable<ICryptoProvider> providers) => _providers = providers;

    public async Task Start(CancellationToken token)
    {
        while (token.IsCancellationRequested is false)
        {
            await Updater();

            await Task.Delay(1000, token)
                .ConfigureAwait(false);
        }
    }

    public async Task Updater()
    {
        foreach (var provider in _providers.AsParallel())
        {
            var rates = await provider.GetAsync()
                .ConfigureAwait(false);

            OnRatesUpdate?.Invoke(provider.Provider, rates);
        }
    }
}
