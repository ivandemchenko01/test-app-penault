using TestApp.Domain.Models;

namespace TestApp.Core.Extensions;


public static class ExchangePairsExtensions
{
    public static ExchangePair GetPair(this List<ExchangePair> pairs, string from, string to)
    {
        string findSymbol = GetSymbol(from, to);

        var result = pairs
            .Find(pair => pair.Symbol.ToLower() == findSymbol);

        if (result is null)
        {
            findSymbol = GetSymbol(to, from);
            result = pairs.Find(pair => pair.Symbol.ToLower() == findSymbol);

            if (result is null)
                throw new Exception("Symbol was not found");

            result.Reversed = true;
        }

        return result;
    }

    private static string GetSymbol(string from, string to) => (from + to).ToLower();
}
