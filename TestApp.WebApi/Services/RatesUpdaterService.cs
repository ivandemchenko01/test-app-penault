using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Diagnostics;
using TestApp.Core;
using TestApp.Core.Providers;
using TestApp.Core.Utils;
using TestApp.Domain.Models;

namespace TestApp.WebApi.Services;

public class RatesUpdaterService : IHostedService
{
    private readonly ExchangeRatesUpdater _backgroundParser;
    private readonly List<string> _allowedPairs;


    public readonly ConcurrentDictionary<ECryptoProvider, List<ExchangePair>> Rates = new();
    public readonly ConcurrentDictionary<ECryptoProvider, List<ExchangePair>> Filtered = new();

    public RatesUpdaterService(ExchangeRatesUpdater parser, IOptions<List<string>> allowedCoins)
    {
        _allowedPairs = allowedCoins.Value.Combinations();

        _backgroundParser = parser;
        _backgroundParser.OnRatesUpdate += OnRatesUpdate;
    }

    private void OnRatesUpdate(ECryptoProvider provider, List<ExchangePair> rates)
    {
        Debug.WriteLine(provider.ToString());
        Debug.WriteLine(string.Join("\n", rates));


        Rates[provider] = rates;

        Filtered[provider] = rates
            .Where(rate => _allowedPairs.Contains(rate.Symbol))
            .ToList();

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _backgroundParser.Start(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        
        return Task.CompletedTask;
    }
}
