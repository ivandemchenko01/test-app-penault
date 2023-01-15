using TestApp.Core.Providers;
using TestApp.Domain.Models;

namespace TestApp.Core.Interfaces
{
    public interface ICryptoProvider
    {
        ECryptoProvider Provider { get; }
        Task<List<ExchangePair>> GetAsync();

    }
}
