using TestApp.Core.Extensions;
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

        var rates = new Dictionary<ECryptoProvider, ExchangePair>();

        foreach (var provider in _ratesService.Filtered.AsParallel())
        {
            var pairs = provider.Value;

            var pair = pairs.GetPair(query.InputCurrency, query.OutputCurrency);

            rates.Add(provider.Key, pair);
        }

        if (rates.Count == 0)
            throw new ArgumentException($"Pairs not found.");

        var prices = new Dictionary<ECryptoProvider, decimal>();

        foreach (var pair in rates)
        {
            decimal price = 0;
            if (pair.Value.Reversed)
                price = query.InputAmount / pair.Value.Rate;
            else
                price = query.InputAmount * pair.Value.Rate;

            prices.Add(pair.Key, price);
        }


        var max = prices.MaxBy(price => price.Value);


        return new Estimate
        {
            Name = max.Key.ToString(),
            OutputAmount = max.Value
        };
    }

    

    private ExchangePair FindPair(List<ExchangePair> pairs, string pair)
    {
        return pairs.Find(p => p.Symbol.ToLower() == pair.ToLower());
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
            var neededPair = provider.Value.Find(pair => pair.Symbol == query.BaseCurrency + query.QuoteCurrency);

            if (neededPair is null)
            {
                neededPair = provider.Value.Find(pair => pair.Symbol == query.QuoteCurrency + query.BaseCurrency);

                decimal price = (decimal)1.0 / neededPair.Rate;

                neededPair.Rate = price;                
            }

            if (neededPair is not null)
                rates.Add(new Rate
                {
                    ExchangeName = provider.Key.ToString(),
                    Value = neededPair.Rate
                });
        }

        return rates;

    }
}
