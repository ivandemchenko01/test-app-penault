using TestApp.Core.Providers;
using TestApp.Domain.Models;
using TestApp.WebApi.Queries;
using TestApp.WebApi.Services.Interfaces;

namespace TestApp.WebApi.Services;

public class ExchangeService : IExchangeService
{
    private readonly RatesUpdaterService _ratesService;
    public ExchangeService(RatesUpdaterService ratesUpdater)
    {
        _ratesService = ratesUpdater;
    }

    public async Task<Estimate> EstimateAsync(EstimateQuery query)
    {
        if (!_ratesService.Filtered.Any())
            throw new Exception($"No pairs on server");

        var rates = new Dictionary<ECryptoProvider, decimal>();
        string findPair = query.InputCurrency + query.OutputCurrency;

        foreach (var provider in _ratesService.Filtered.AsParallel())
        {
            var rate = provider.Value.Find(pair => pair.Symbol == findPair);
            if (rate is null)
                continue;

            rates.Add(provider.Key, rate.Rate);
        }

        if (rates.Count == 0)
            throw new ArgumentException($"Pair {findPair} not found.");

        var max = rates.MaxBy(rate => rate.Value);

        return new Estimate
        {
            Name = max.Key.ToString(),
            OutputAmount = max.Value * query.InputAmount
        };
    }

    public async Task<List<Rate>> GetRatesAsync(GetRatesQuery query)
    {
        string pair = query.BaseCurrency + query.QuoteCurrency;
        pair = pair.ToUpper();

        if (!_ratesService.Filtered.Any())
            throw new Exception($"Pair {pair} not found on server.");

        var rates = new List<Rate>();

        foreach (var provider in _ratesService.Filtered.AsParallel())
        {
            foreach (var pairRate in provider.Value)
                if (pairRate.Symbol.ToUpper() == pair)
                    rates.Add(new Rate
                    {
                        ExchangeName = provider.Key.ToString(),
                        Value = pairRate.Rate
                    });
        }

        return rates;

    }
}
