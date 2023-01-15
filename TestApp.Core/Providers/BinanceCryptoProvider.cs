using BinanceExchange.API.Client;
using BinanceExchange.API.Models.Response;
using Microsoft.Extensions.Options;
using TestApp.Core.Configuration;
using TestApp.Core.Interfaces;
using TestApp.Core.Utils;
using TestApp.Domain.Models;

namespace TestApp.Core.Providers;

public class BinanceCryptoProvider : ICryptoProvider
{
    private readonly BinanceClient _client;
    private readonly BinanceOptions _options;
    private readonly List<string> _coins;

    public ECryptoProvider Provider => ECryptoProvider.Binance;

    public BinanceCryptoProvider(IOptions<BinanceOptions> options, IOptions<List<string>> allowedCoins)
    {
        _coins = allowedCoins.Value.Combinations();

        _options = options.Value;
        _client = new BinanceClient(new()
        {
            ApiKey = _options.Key,
            SecretKey = _options.SecretKey,
        });
    }


    public async Task<List<ExchangePair>> GetAsync()
    {
        var prices = await _client.GetAllPrices();
        prices = prices.
            Where(price => _coins.Contains(price.Symbol))
            .ToList();

        var list = new List<SymbolPriceResponse>();
        foreach (var price in prices)
            list.Add(await _client.GetPrice(price.Symbol));


        return prices
            .Select(ConvertToPair)
            .ToList();
    }

    private ExchangePair ConvertToPair(SymbolPriceResponse response)
    {
        return new ExchangePair()
        {
            Rate = response.Price,
            Symbol = response.Symbol
        };
    }


}
