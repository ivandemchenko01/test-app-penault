using CryptoExchange.Net.Authentication;
using Kuna.Net;
using Kuna.Net.Interfaces;
using Kuna.Net.Objects.V3;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;
using TestApp.Core.Configuration;
using TestApp.Core.Interfaces;
using TestApp.Domain.Models;

namespace TestApp.Core.Providers;


public class KunaCryptoProvider : ICryptoProvider
{
    private readonly IKunaClient _kunaClient;
    private readonly KunaOptions _options;

    public KunaCryptoProvider(IOptions<KunaOptions> options)
    {
        _kunaClient = new KunaClient();

        var publicKey = new NetworkCredential("", options.Value.Key).SecurePassword;
        var secretKey = new NetworkCredential("", options.Value.SecretKey).SecurePassword;

        _kunaClient.SetApiCredentials(new ApiCredentials(publicKey, secretKey));
    }

    public ECryptoProvider Provider => ECryptoProvider.Kuna;



    public async Task<List<ExchangePair>> GetAsync()
    {
       try
        {
            var testPairs = await _kunaClient.ClientV3.GetTradingPairsAsync()
                .ConfigureAwait(false);
            
            var parameters = testPairs.Data.Select(pair => pair.Pair).ToArray();

            var prices = await _kunaClient.ClientV3.GetTickersAsync(symbols: parameters);

            return prices.Data
                .Select(ConvertToPair)
                .ToList();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception");
        }
        return new List<ExchangePair>();
    }

    private ExchangePair ConvertToPair(KunaTicker ticker)
    {
        return new ExchangePair
        {
            Symbol = ticker.Symbol.ToUpper(),
            Rate = ticker.Ask
        };
    }
}
