using TestApp.Domain.Models;
using TestApp.WebApi.Queries;

namespace TestApp.WebApi.Services.Interfaces;

public interface IExchangeService
{
    Task<Estimate> EstimateAsync(EstimateQuery query);
    Task<List<Rate>> GetRatesAsync(GetRatesQuery query);
}
