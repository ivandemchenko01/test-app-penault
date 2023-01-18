using Binance.Net.Clients;
using CryptoExchange.Net.Authentication;
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
        _client = new BinanceClient(new() { ApiCredentials = new ApiCredentials(options.Value.Key, options.Value.SecretKey)});
    }


    public async Task<List<ExchangePair>> GetAsync()
    {
        var prices = await _client.SpotApi.ExchangeData.GetTickersAsync();

        var neededCoins = prices.Data
            .Where(price => _coins.Contains(price.Symbol.Replace("-", "")))
            .ToList();

        var pairs =  neededCoins
            .Select(price => new ExchangePair
            {
                Symbol = price.Symbol,
                Rate = (decimal)price.LastPrice,
                
            })
            .ToList();

        return pairs;
    }



}
