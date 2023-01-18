using Kucoin.Net.Clients;
using Kucoin.Net.Objects;
using Microsoft.Extensions.Options;
using TestApp.Core.Configuration;
using TestApp.Core.Interfaces;
using TestApp.Core.Utils;
using TestApp.Domain.Models;

namespace TestApp.Core.Providers;

public class KucoinCryptoProvider : ICryptoProvider
{
    private readonly KucoinClient _kucoinClient;
    private readonly KucoinOptions _options;
    private readonly List<string> _coins;
    public ECryptoProvider Provider => ECryptoProvider.Kucoin;

    public KucoinCryptoProvider(
        IOptions<KucoinOptions> options, 
        IOptions<List<string>> allowedCoins)
    {
        _coins = allowedCoins.Value.Combinations();

        _kucoinClient = new KucoinClient(new KucoinClientOptions
        {
            ApiCredentials = new KucoinApiCredentials(options.Value.Key, options.Value.Secret, options.Value.SecretPhrase)
        });

        _options = options.Value;
    }


    public async Task<List<ExchangePair>> GetAsync()
    {
        var price = await _kucoinClient.SpotApi.CommonSpotClient.GetTickersAsync();

        var pairs = price.Data
            .Where(price => _coins.Contains(price.Symbol.Replace("-", "")))
            .Select(price => new ExchangePair
            {
                Symbol = price.Symbol.Replace("-", ""),
                Rate = (decimal)price.LastPrice
            })
            .ToList();

        return pairs;
    }


}
